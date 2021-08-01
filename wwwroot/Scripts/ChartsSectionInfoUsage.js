var usageInfo = document.getElementById("usageInfo");
var usageInfoContext = usageInfo.getContext("2d");
var usageInfoSection = document.getElementById("uInfoBody");
var usageInfoSectionH = document.getElementById("uInfoHeader");
var usageInfoHeader = document.getElementById("usageInfoHeader");
var usageInfoHContext = usageInfoHeader.getContext("2d");

generateChartSignature();
updateUsageInfoSection();

function updateUsageInfoSection(){
    generateTimeForInfoSection();
    generateUInfoReadings();
    drawGridForUsageChart();
    generateMinMaxUValue();
}

function drawGridForUsageChart(){
    drawHorizontalLanesForUC();
    drawVerticalLanesForUC();
}

function generateMinMaxUValue(){
    signMinMaxUsageValues("Min. usage: ", "green", 50, 115);
    signMinMaxUsageValues(viewmodel.cpu.lowestUsage+"%", "blue", 130, 115);
    signMinMaxUsageValues(viewmodel.memory.lowestUsage+"%", "teal", 170, 115);
    signMinMaxUsageValues("Max. usage: ", "red", 220, 115);
    signMinMaxUsageValues(viewmodel.cpu.highestUsage+"%", "blue", 305, 115);
    signMinMaxUsageValues(viewmodel.memory.highestUsage+"%", "teal", 345, 115);
    signMinMaxUsageValues("(Values for last 24 hours)", "gray", 395, 115);

    signMinMaxUsageValuesT("Min. temp: ", "green", 50, 115);
    signMinMaxUsageValuesT(viewmodel.cpu.lowestTemp+"°C", "blue", 130, 115);
    signMinMaxUsageValuesT(viewmodel.mobo.lowestTemp+"°C", "darkmagenta", 170, 115);
    signMinMaxUsageValuesT("Max. temp: ", "red", 230, 115);
    signMinMaxUsageValuesT(viewmodel.cpu.highestTemp+"°C", "blue", 305, 115);
    signMinMaxUsageValuesT(viewmodel.mobo.highestTemp+"°C", "darkmagenta", 345, 115);
    signMinMaxUsageValuesT("(Values for last 24 hours)", "gray", 395, 115);
}

function drawHorizontalLanesForUC(){
    for (i = 1; i <= 3; i++) {
        var currentHeightCU = i*30;
        usageInfoContext.beginPath();
        usageInfoContext.strokeStyle = 'rgba(191, 191, 191, .5)';
        usageInfoContext.lineWidth = 1;
        usageInfoContext.moveTo(0, currentHeightCU + 1);
        usageInfoContext.lineTo(usageInfoSection.clientWidth, currentHeightCU + 1);
        usageInfoContext.stroke();
    }
}

function drawVerticalLanesForUC(){
    for (i = 1; i < 20; i++) {
        var currentWidthCU = usageInfoSection.clientWidth - (i * gridWidth + 1.5);
        usageInfoContext.beginPath();
        usageInfoContext.strokeStyle = 'rgba(191, 191, 191, .5)';
        usageInfoContext.lineWidth = 1;
        usageInfoContext.moveTo(currentWidthCU, 0);
        usageInfoContext.lineTo(currentWidthCU, 90);
        usageInfoContext.stroke();
    }
}

function generateChartSignature(){
    debugger;
    usageInfoHeader.height = usageInfoSectionH.clientHeight;
    usageInfoHeader.width = usageInfoSectionH.clientWidth;
    var signatureX = (usageInfoHeader.width/2) - 100;
    var signatureY = usageInfoHeader.height/2;
    drawHeaderSignature("CPU", "blue", signatureX, signatureY);
    drawHeaderSignature("Memory", "teal", signatureX + 100, signatureY);
}

function drawHeaderSignature(text, fontColor, x, y){
    usageInfoHContext.beginPath();
    usageInfoHContext.strokeStyle = fontColor;
    usageInfoHContext.lineWidth = 4;
    usageInfoHContext.moveTo(x, y);
    usageInfoHContext.lineTo(x + 50, y);
    usageInfoHContext.stroke();
    drawTextHeader(text, x, y);
}

function drawTextHeader(text, x, y){
    usageInfoHContext.fillStyle = "white";
    usageInfoHContext.font = "12px Arial";
    usageInfoHContext.fillText(text, x + 60, y+3);
}

function generateUInfoReadings(){
    var limitter = viewmodel.cpu.usageReadings.length;
    for (i = 0; i < limitter; i++) {
        if(limitter == 21)
        {
            limitter--;
        }
        var currentReadingC = viewmodel.cpu.usageReadings[limitter-(i+1)].usage;
        var currentReadingM = viewmodel.memory.usageReadings[limitter-(i+1)].usage;
        currentWidth = usageInfoSection.clientWidth - ((i+1) * gridWidth - 1);

        signAxisForInfoSection(currentReadingC+'%', "blue", currentWidth, 50);
        signAxisForInfoSection(currentReadingM+'%', "teal", currentWidth, 80);
    }
}

function generateTimeForInfoSection() {
    usageInfo.height = usageInfoSection.clientHeight;
    usageInfo.width = usageInfoSection.clientWidth;
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
        currentWidth = usageInfoSection.clientWidth - (x * gridWidth - 1);

        signAxisForInfoSection(currentTime, "white", currentWidth, 20);
    }
}

function signAxisForInfoSection(text, fontColor, currentWidth, height) {
    usageInfoContext.fillStyle = fontColor;
    usageInfoContext.font = "10px Arial";
    usageInfoContext.fillText(text, currentWidth, height);
}

function signMinMaxUsageValues(text, fontColor, currentWidth, height){
    usageInfoContext.fillStyle = fontColor;
    usageInfoContext.font = "14px Arial";
    usageInfoContext.fillText(text, currentWidth, height);
}