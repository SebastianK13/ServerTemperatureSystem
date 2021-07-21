const cpuUsage = document.getElementById("ccuUsage");
const ctx = cpuUsage.getContext('2d');
const threePIByTwo = (Math.PI);
const activeColor = '#FDAC42',
    inactiveColor = '#282A2D';
var chartX = 0;
var chartY = 0;
var result = 0;
var cpuUsageLabel = document.getElementById("cpuUsageLblId");

initialize();

function setUsageValue(x) {
    result = (180 * x) / 100;
}

function initialize() {
    setUsageValue(82);
    cpuUsage.height = 140;
    chartX = cpuUsage.width / 2;
    chartY = 130;
    animateOnInit();
    currentDistance = 0;
    cpuUsageLabel.innerHTML = viewmodel.cpu.usage+"%";
}

function animateOnInit(){
    cycles = Math.ceil((18*result) / 180);
    drawCircle(chartX, chartY, 100, Math.PI, 2 * Math.PI, false, inactiveColor, 25);

    for(i=0;i<=result;i++)
    {
        setDelay(i);
    }
    finished = true;
}
function setDelay(i) {
    setTimeout(function(){
        distance = i;
        drawBar(distance);
    }, 5 * i);
}
function drawBar(distance){
    drawCircle(chartX, chartY, 100, Math.PI, setRadians(distance) + Math.PI, false, activeColor, 25);
}

function setRadians(deg) {
    return (Math.PI / 180) * deg;
}

function drawArc(x, y, radius, start, end, clockwise) {
    ctx.beginPath();
    ctx.arc(x, y, radius, start, end, clockwise);
}

function drawCircle(x, y, radius, start, end, clockwise, color, thickness) {
    ctx.strokeStyle = color;
    ctx.lineWidth = thickness;
    drawArc(x, y, radius, start, end, clockwise)
    ctx.stroke();
}