const chartUsage = document.getElementById("coresCpuChartU");
const chartUCtx = chartUsage.getContext("2d");
const area = document.getElementById("cuUsage");
var cpuUX = 0;
var cpuUY = 0;
var gridLineHeight = 0;
var gridWidth = 0;
var timeRange = [];
var lastX = 0 ;
var lastCpuY = 0;
var lastMemY = 0;
var previousReadings;
var lastY = 0;

function updateGridChart()
{
    var x = 0;
    for(i = 0; i < viewmodel.cpu.usageReadings.length; i++)
    {  
        if(i<20)
        {
            var cpuY = (viewmodel.cpu.usageReadings[i].usage * (cpuUY-30))/100;
            var memY = (viewmodel.memory.usageReadings[i].usage * (cpuUY-30))/100;
                if (i == 0)
                {
                    lastX = ((i+1) * gridWidth - 1);
                    lastCpuY = viewmodel.cpu.usageReadings[i+1].usage;
                    lastMemY = viewmodel.memory.usageReadings[i+1].usage;
                }
                else if(i == 19)
                {
                    x = ((i) * gridWidth - 1) - 3;
                    drawCPUUsageLine(x, cpuY);
                    drawMemoryUsageLine(x, memY);
                }
                else
                {
                    x = ((i) * gridWidth - 1);
                    drawCPUUsageLine(x, cpuY);
                    drawMemoryUsageLine(x, memY);
                }
            lastX = x;
            lastCpuY = cpuY;
            lastMemY = memY;
        }
    }
    generatePickPoints();
}

function generatePickPoints()
{
    var x = 0;
    for(i = 0; i < viewmodel.cpu.usageReadings.length; i++)
    {  
        if(i<20)
        {
            var cpuY = (viewmodel.cpu.usageReadings[i].usage * (cpuUY-30))/100;;
            var memY = (viewmodel.memory.usageReadings[i].usage * (cpuUY-30))/100;;
                if (i == 0)
                {
                    lastX = ((i+1) * gridWidth - 1);
                    lastCpuY = viewmodel.cpu.usageReadings[i+1].usage;
                    lastMemY = viewmodel.memory.usageReadings[i+1].usage;
                    drawCpuPoint(x, cpuY);
                    drawMemoryPoint(x, memY);
                }
                else if(i == 19)
                {
                    x = ((i) * gridWidth - 1) - 3;
                    drawCpuPoint(x, cpuY);
                    drawMemoryPoint(x, memY);
                }
                else
                {
                    x = ((i) * gridWidth - 1);
                    drawCpuPoint(x, cpuY);
                    drawMemoryPoint(x, memY);
                }
            lastX = x;
            lastCpuY = cpuY;
            lastMemY = memY;
        }
    }
}

function drawCPUUsageLine(x,y) {
    chartUCtx.beginPath();
    chartUCtx.strokeStyle = "blue";
    chartUCtx.lineWidth = 2;
    chartUCtx.moveTo(lastX+34, chartUsage.height - (lastCpuY+30));
    chartUCtx.lineTo(x+34, chartUsage.height - (y+30));
    chartUCtx.stroke();
}

function drawMemoryUsageLine(x,y) {
    chartUCtx.beginPath();
    chartUCtx.strokeStyle = "teal";
    chartUCtx.lineWidth = 2;
    chartUCtx.moveTo(lastX+34, chartUsage.height - (lastMemY+30));
    chartUCtx.lineTo(x+34, chartUsage.height - (y+30));
    chartUCtx.stroke();
}

function drawCpuPoint(x,y) {
    chartUCtx.beginPath();
    chartUCtx.fillStyle = "white";
    chartUCtx.arc(x+34, chartUsage.height - (y+30), 2, 0, 2 * Math.PI, true);
    chartUCtx.fill();
}
function drawMemoryPoint(x,y) {
    chartUCtx.beginPath();
    chartUCtx.fillStyle = "white";
    chartUCtx.arc(x+34, chartUsage.height - (y+30), 2, 0, 2 * Math.PI, true);
    chartUCtx.fill();
}

generateCurve();

function generateCurve() {
    cpuUX = area.clientWidth;
    cpuUY = area.clientHeight;
    chartUsage.height = cpuUY;
    chartUsage.width = cpuUX;
    drawGraphGrid();
    updateGridChart();
}

function drawGraphGrid() {
    drawHorizontalLanes();
    drawVerticalLanes();
}

function drawHorizontalLanes() {
    gridLineHeight = (cpuUY - 30) / 5;
    for (i = 0; i <= 5; i++) {
        currentHeight = 0;

        if (i == 0)
            currentHeight = cpuUY - 1;
        else
        {
            currentHeight = cpuUY - (i * gridLineHeight - 1);
            signAxisY(i*20, currentHeight);
        }

        chartUCtx.beginPath();
        chartUCtx.strokeStyle = 'rgba(255,255,255, .2)';
        chartUCtx.lineWidth = 1;
        chartUCtx.moveTo(34, currentHeight - 30);
        chartUCtx.lineTo(cpuUX-4, currentHeight - 30);
        chartUCtx.stroke();
    }
}

function signAxisY(axisNum, currentHeight){
    chartUCtx.fillStyle = "white";
    chartUCtx.font = "10px Arial";
    chartUCtx.fillText(axisNum, 5, currentHeight);
}

function signAxisX(axisNum, currentWidth){
    chartUCtx.fillStyle = "white";
    chartUCtx.font = "10px Arial";
    chartUCtx.fillText(axisNum, currentWidth+1, cpuUY - 10);
}

function drawVerticalLanes() {
    gridWidth = (cpuUX - 34) / 19;
    var difference = 0;
    var finalPoint = 0;
    generateHoursAndMinutes();
    for (i = 1; i <= 20; i++) {
        currentWidth = cpuUX - ((i-1) * gridWidth - 1);
        difference = 32;
        finalPoint = 2;

        if (i == 1)
        {
            currentWidth = cpuUX - 1;
            difference = 30;
            finalPoint = 0;
        }
        else if(i == 20)
        {
            difference = 30;
            finalPoint = 0;
        }

        chartUCtx.beginPath();
        chartUCtx.strokeStyle = 'rgba(255,255,255, .2)';
        chartUCtx.lineWidth = 1;
        chartUCtx.moveTo(currentWidth - 2, cpuUY - difference);
        chartUCtx.lineTo(currentWidth - 2, finalPoint);
        chartUCtx.stroke();
    }
}
function generateHoursAndMinutes(){
    var date = new Date();
    var hours = parseInt(date.getHours());
    var minutes = (hours * 60) + parseInt(date.getMinutes());
    var currMinutes = 0;
    for(x = 1; x <= 19; x++){
        minutes -= 1;
        roundedH = Math.floor(minutes/60);
        currMinutes = minutes - (roundedH *60);

        let hoursString = roundedH.toString();
        if(hoursString.length == 1)
            hoursString = "0" + roundedH;

        let minutesString = currMinutes.toString();
        if(minutesString.length == 1)
            minutesString = "0" + currMinutes;

        var currentTime = hoursString+":"+minutesString;
        timeRange.push(currentTime);
        currentWidth = cpuUX - (x * gridWidth - 1);

        signAxisX(currentTime, currentWidth);
    }
}