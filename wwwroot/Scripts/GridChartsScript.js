const chartUsage = document.getElementById("coresCpuChartU");
const chartUCtx = chartUsage.getContext("2d");
const area = document.getElementById("cuUsage");
var cpuUX = 0;
var cpuUY = 0;
var gridLineHeight = 0;
var gridWidth = 0;
var timeRange = [];
var lastX = 0 ;
var lastY = 0;
var previousReadings;

function updateGridChart()
{
    //readings update implementation
    var x = 0;
    lastX = 0;
    lastY = 0;
    for(i = 0; i < 20; i++)
    {  
            if (i == 0)
            {
                //generateHoursAndMinutes();
                x = gridWidth - 1;
            }
            else
            {
                x = ((i+1) * gridWidth - 1);
            }
        var y = viewmodel.cpu.usageReadings[i].usage;
        drawLine(x, y);
        drawPoint(x, y);
        lastX = x;
        lastY = y;
    }
}

function drawLine(x,y) {
    chartUCtx.beginPath();
    chartUCtx.strokeStyle = "blue";
    chartUCtx.lineWidth = 1;
    chartUCtx.moveTo(lastX+34, chartUsage.height - (lastY+30));
    chartUCtx.lineTo(x+34, chartUsage.height - (y+30));
    chartUCtx.stroke();
}

function drawPoint(x,y) {
    chartUCtx.beginPath();
    chartUCtx.fillStyle = "white";
    chartUCtx.arc(x+32, chartUsage.height - (y+30), 3, 0, 2 * Math.PI, true);
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
        chartUCtx.strokeStyle = 'rgba(255,255,255, .5)';
        chartUCtx.lineWidth = 1;
        chartUCtx.moveTo(32, currentHeight - 30);
        chartUCtx.lineTo(cpuUX-2, currentHeight - 30);
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
    gridWidth = (cpuUX - 30) / 20;
    var difference = 0;
    var finalPoint = 0;
    for (i = 0; i <= 20; i++) {
        currentWidth = cpuUX - (i * gridWidth - 1);
        difference = 32;
        finalPoint = 2;

        if (i == 0)
        {
            generateHoursAndMinutes();
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
        chartUCtx.strokeStyle = 'rgba(255,255,255, .5)';
        chartUCtx.lineWidth = 1;
        chartUCtx.moveTo(currentWidth, cpuUY - difference);
        chartUCtx.lineTo(currentWidth, finalPoint);
        chartUCtx.stroke();
    }
}
function generateHoursAndMinutes(){
    gridWidth = (cpuUX - 30) / 20;
    var date = new Date();
    var hours = parseInt(date.getHours());
    var minutes = (hours * 60) + parseInt(date.getMinutes());
    var currMinutes = 0;
    for(x = 1; x <= 20; x++){
        minutes -= 3;
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