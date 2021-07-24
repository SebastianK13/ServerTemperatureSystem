const chartTemp = document.getElementById("coresCpuChartU");
const chartTCtx = chartTemp.getContext("2d");
const area = document.getElementById("cuUsage");
var cpuUX = 0;
var cpuUY = 0;
var gridLineHeight = 0;
var gridWidth = 0;
var timeRange = [];

generateCurve();

function generateCurve() {
    cpuUX = area.clientWidth;
    cpuUY = area.clientHeight;
    chartTemp.height = cpuUY;
    chartTemp.width = cpuUX;
    drawGraphGrid();
    drawLine();
}

function drawLine() {
    chartTCtx.beginPath();
    chartTCtx.strokeStyle = "blue";
    chartTCtx.lineWidth = 1;
    chartTCtx.moveTo(31, cpuUY - 31);
    chartTCtx.lineTo(100, 150);
    chartTCtx.stroke();
    drawPoint();
}

function drawPoint() {
    chartTCtx.beginPath();
    chartTCtx.fillStyle = "white";
    chartTCtx.arc(100, 150, 3, 0, 2 * Math.PI, true);
    chartTCtx.fill();
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

        chartTCtx.beginPath();
        chartTCtx.strokeStyle = "whitesmoke";
        chartTCtx.lineWidth = 1;
        chartTCtx.moveTo(30, currentHeight - 30);
        chartTCtx.lineTo(cpuUX, currentHeight - 30);
        chartTCtx.stroke();
    }
}

function signAxisY(axisNum, currentHeight){
    chartTCtx.fillStyle = "white";
    chartTCtx.font = "10px Arial";
    chartTCtx.fillText(axisNum, 5, currentHeight);
}

function signAxisX(axisNum, currentWidth){
    chartTCtx.fillStyle = "white";
    chartTCtx.font = "10px Arial";
    chartTCtx.fillText(axisNum, currentWidth+1, cpuUY - 10);
}

function drawVerticalLanes() {
    gridWidth = (cpuUX - 30) / 20;
    for (i = 0; i <= 20; i++) {
        currentWidth = 0;

        if (i == 0)
        {
            generateHoursAndMinutes();
            currentWidth = cpuUX - 1;
        }
        else
        {
            currentWidth = cpuUX - (i * gridWidth - 1);
        }

        chartTCtx.beginPath();
        chartTCtx.strokeStyle = "whitesmoke";
        chartTCtx.lineWidth = 1;
        chartTCtx.moveTo(currentWidth, cpuUY - 30);
        chartTCtx.lineTo(currentWidth, 0);
        chartTCtx.stroke();
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