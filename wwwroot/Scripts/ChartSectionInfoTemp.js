var tempInfo = document.getElementById("tempInfo");
var tempInfoContext = tempInfo.getContext("2d");
var tempInfoSection = document.getElementById("tInfoBody");
var tempInfoSectionH = document.getElementById("tInfoHeader");
var tempInfoHeader = document.getElementById("tempInfoHeader");
var tempInfoHContext = tempInfoHeader.getContext("2d");

generateChartSignatureT();
updateTempInfoSection();

function updateTempInfoSection(){
    generateTimeForInfoSectionT();
    generateUInfoReadingsT();
    drawGridForTempChart();
    generateMinMaxUValueT();
}

function drawGridForTempChart(){
    drawHorizontalLanesForTC();
    drawVerticalLanesForTC();
}

function generateMinMaxUValueT(){
    signMinMaxUsageValuesT("Min. temp: ", "green", 50, 115);
    signMinMaxUsageValuesT(viewmodel.cpu.lowestTemp+"°C", "blue", 130, 115);
    signMinMaxUsageValuesT(viewmodel.mobo.lowestTemp+"°C", "darkmagenta", 170, 115);
    signMinMaxUsageValuesT("Max. temp: ", "red", 230, 115);
    signMinMaxUsageValuesT(viewmodel.cpu.highestTemp+"°C", "blue", 305, 115);
    signMinMaxUsageValuesT(viewmodel.mobo.highestTemp+"°C", "darkmagenta", 345, 115);
    signMinMaxUsageValuesT("(Values for last 24 hours)", "gray", 395, 115);
}

function drawHorizontalLanesForTC(){
    for (i = 1; i <= 3; i++) {
        var currentHeightCU = i*30;
        tempInfoContext.beginPath();
        tempInfoContext.strokeStyle = 'rgba(191, 191, 191, .5)';
        tempInfoContext.lineWidth = 1;
        tempInfoContext.moveTo(0, currentHeightCU + 1);
        tempInfoContext.lineTo(tempInfoSection.clientWidth, currentHeightCU + 1);
        tempInfoContext.stroke();
    }
}

function drawVerticalLanesForTC(){
    for (i = 1; i < 20; i++) {
        var currentWidthCU = tempInfoSection.clientWidth - (i * gridWidth + 1.5);
        tempInfoContext.beginPath();
        tempInfoContext.strokeStyle = 'rgba(191, 191, 191, .5)';
        tempInfoContext.lineWidth = 1;
        tempInfoContext.moveTo(currentWidthCU, 0);
        tempInfoContext.lineTo(currentWidthCU, 90);
        tempInfoContext.stroke();
    }
}

function generateChartSignatureT(){
    tempInfoHeader.height = tempInfoSectionH.clientHeight;
    tempInfoHeader.width = tempInfoSectionH.clientWidth;
    var signatureXT = (tempInfoHeader.width/2) - 100;
    var signatureYT = tempInfoHeader.height/2;
    drawHeaderSignatureT("CPU", "blue", signatureXT, signatureYT);
    drawHeaderSignatureT("Mobo", "darkmagenta", signatureXT + 100, signatureYT);
}

function drawHeaderSignatureT(text, fontColor, x, y){
    tempInfoHContext.beginPath();
    tempInfoHContext.strokeStyle = fontColor;
    tempInfoHContext.lineWidth = 4;
    tempInfoHContext.moveTo(x, y);
    tempInfoHContext.lineTo(x + 50, y);
    tempInfoHContext.stroke();
    drawTextHeaderT(text, x, y);
}

function drawTextHeaderT(text, x, y){
    tempInfoHContext.fillStyle = "white";
    tempInfoHContext.font = "12px Arial";
    tempInfoHContext.fillText(text, x + 60, y+3);
}

function generateUInfoReadingsT(){
    var limitter = viewmodel.cpu.temperatureReadings.length;
    for (i = 0; i < limitter; i++) {
        if(limitter == 21)
        {
            limitter--;
        }
        var currentReadingC = viewmodel.cpu.temperatureReadings[limitter-(i+1)].temperature;
        var currentReadingM = parseInt(viewmodel.mobo.temperatureReadings[limitter-(i+1)].temperature);
        currentWidth = tempInfoSection.clientWidth - ((i+1) * gridWidth - 1);

        signAxisForInfoSectionT(currentReadingC+'°C', "blue", currentWidth, 50);
        signAxisForInfoSectionT(currentReadingM+'°C', "darkmagenta", currentWidth, 80);
    }
}

function generateTimeForInfoSectionT() {
    tempInfo.height = tempInfoSection.clientHeight;
    tempInfo.width = tempInfoSection.clientWidth;
    var date = new Date();
    var hours = parseInt(date.getHours());
    var minutes = (hours * 60) + parseInt(date.getMinutes());
    if (hours == 0)
        minutes = (24 * 60) + parseInt(date.getMinutes());

    var currMinutes = 0;
    for (x = 1; x <= 20; x++) {
        minutes -= 1;
        roundedH = Math.floor(minutes / 60);
        currMinutes = minutes - (roundedH * 60);

        let hoursString = roundedH.toString();
        if (hoursString.length == 1)
            hoursString = "0" + roundedH;

        let minutesString = currMinutes.toString();
        if (minutesString.length == 1)
            minutesString = "0" + currMinutes;

        if (hours == 0)
            hoursString = "00";

        var currentTime = hoursString + ":" + minutesString;
        timeRange.push(currentTime);
        currentWidth = tempInfoSection.clientWidth - (x * gridWidth - 1);

        signAxisForInfoSectionT(currentTime, "white", currentWidth, 20);
    }
}

function signAxisForInfoSectionT(text, fontColor, currentWidth, height) {
    tempInfoContext.fillStyle = fontColor;
    tempInfoContext.font = "10px Arial";
    tempInfoContext.fillText(text, currentWidth, height);
}

function signMinMaxUsageValuesT(text, fontColor, currentWidth, height){
    tempInfoContext.fillStyle = fontColor;
    tempInfoContext.font = "14px Arial";
    tempInfoContext.fillText(text, currentWidth, height);
}