const chartTemp = document.getElementById("moboCpuChartT");
const chartTCtx = chartTemp.getContext("2d");
const areaT = document.getElementById("mbcTemp");
/* var cpuUX = 0;
var cpuUY = 0;
var gridLineHeight = 0;
var gridWidth = 0;
var timeRange = [];
var lastX = 0 ;
var lastCpuY = 0;
var lastMemY = 0;
var previousReadings;
var lastY = 0; */

generateCurveT();

function updateTempGridChart()
{
    var x = 0;
    for(i = 0; i < viewmodel.mobo.temperatureReadings.length; i++)
    {  
        if(i < 20)
        {
            var moboYT = (viewmodel.mobo.temperatureReadings[i].temperature * (cpuUY-30))/100;
            var cpuYT = (viewmodel.cpu.temperatureReadings[i].temperature * (cpuUY-30))/100;

                if (i == 0)
                {
                    lastX = ((i+1) * gridWidth - 1);
                    lastCpuY = viewmodel.cpu.temperatureReadings[i+1].temperature;
                    lastMemY = viewmodel.mobo.temperatureReadings[i+1].temperature;
                }
                else if(i == 19)
                {
                    x = ((i) * gridWidth - 1) - 3;
                    drawCPUTempLine(x, cpuYT);
                    drawMoboTempLine(x, moboYT);
                }
                else
                {
                    x = ((i) * gridWidth - 1);
                    drawCPUTempLine(x, cpuYT);
                    drawMoboTempLine(x, moboYT);
                }
            lastX = x;
            lastCpuY = cpuYT;
            lastMemY = moboYT;
        }
    }
    generatePickPointsT()
}

function generatePickPointsT()
{
    var x = 0;
    for(i = 0; i < viewmodel.cpu.temperatureReadings.length; i++)
    {  
        if(i<20)
        {
            var cpuY = (viewmodel.cpu.temperatureReadings[i].temperature * (cpuUY-30))/100;
            var memY = (viewmodel.mobo.temperatureReadings[i].temperature * (cpuUY-30))/100;;
                if (i == 0)
                {
                    lastX = ((i+1) * gridWidth - 1);
                    lastCpuY = viewmodel.cpu.temperatureReadings[i].temperature;
                    lastMemY = viewmodel.mobo.temperatureReadings[i].temperature;
                    drawCpuPointT(x, cpuY);
                    drawMoboPoint(x, memY);
                }
                else if(i == 19)
                {
                    x = ((i) * gridWidth - 1) - 3;
                    drawCpuPointT(x, cpuY);
                    drawMoboPoint(x, memY);
                }
                else
                {
                    x = ((i) * gridWidth - 1);
                    drawCpuPointT(x, cpuY);
                    drawMoboPoint(x, memY);
                }
            lastX = x;
            lastCpuY = cpuY;
            lastMemY = memY;
        }

    }
}

function drawCPUTempLine(x,y) {
    chartTCtx.beginPath();
    chartTCtx.strokeStyle = "blue";
    chartTCtx.lineWidth = 2;
    chartTCtx.moveTo(lastX+34, chartTemp.height - (lastCpuY+30));
    chartTCtx.lineTo(x+34, chartTemp.height - (y+30));
    chartTCtx.stroke();
}

function drawMoboTempLine(x,y) {
    chartTCtx.beginPath();
    chartTCtx.strokeStyle = "red";
    chartTCtx.lineWidth = 2;
    chartTCtx.moveTo(lastX+34, chartTemp.height - (lastMemY+30));
    chartTCtx.lineTo(x+34, chartTemp.height - (y+30));
    chartTCtx.stroke();
}

function drawCpuPointT(x,y) {
    chartTCtx.beginPath();
    chartTCtx.fillStyle = "white";
    chartTCtx.arc(x+34, chartTemp.height - (y+30), 2, 0, 2 * Math.PI, true);
    chartTCtx.fill();
}
function drawMoboPoint(x,y) {
    chartTCtx.beginPath();
    chartTCtx.fillStyle = "white";
    chartTCtx.arc(x+34, chartTemp.height - (y+30), 2, 0, 2 * Math.PI, true);
    chartTCtx.fill();
}

function generateCurveT() {
    cpuUX = areaT.clientWidth;
    cpuUY = areaT.clientHeight;
    chartTemp.height = cpuUY;
    chartTemp.width = cpuUX;
    drawGraphGridT();
    updateTempGridChart();
}

function drawGraphGridT() {
    drawHorizontalLanesT();
    drawVerticalLanesT();
}

function drawHorizontalLanesT() {
    gridLineHeight = (cpuUY - 30) / 5;
    for (i = 0; i <= 5; i++) {
        currentHeight = 0;

        if (i == 0)
            currentHeight = cpuUY - 1;
        else
        {
            currentHeight = cpuUY - (i * gridLineHeight - 1);
            signAxisYT(i*20, currentHeight);
        }

        chartTCtx.beginPath();
        chartTCtx.strokeStyle = 'rgba(255,255,255, .2)';
        chartTCtx.lineWidth = 1;
        chartTCtx.moveTo(34, currentHeight - 30);
        chartTCtx.lineTo(cpuUX-4, currentHeight - 30);
        chartTCtx.stroke();
    }
}

function signAxisYT(axisNum, currentHeight){
    chartTCtx.fillStyle = "white";
    chartTCtx.font = "10px Arial";
    chartTCtx.fillText(axisNum, 5, currentHeight);
}

function signAxisXT(axisNum, currentWidth){
    chartTCtx.fillStyle = "white";
    chartTCtx.font = "10px Arial";
    chartTCtx.fillText(axisNum, currentWidth+1, cpuUY - 10);
}

function drawVerticalLanesT() {
    gridWidth = (cpuUX - 34) / 19;
    var difference = 0;
    var finalPoint = 0;
    generateHoursAndMinutesT();
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

        chartTCtx.beginPath();
        chartTCtx.strokeStyle = 'rgba(255,255,255, .2)';
        chartTCtx.lineWidth = 1;
        chartTCtx.moveTo(currentWidth - 2, cpuUY - difference);
        chartTCtx.lineTo(currentWidth - 2, finalPoint);
        chartTCtx.stroke();
    }

    function generateHoursAndMinutesT(){
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
    
            signAxisXT(currentTime, currentWidth);
        }
    }
}