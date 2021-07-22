const cpuUsage = document.getElementById("ccuUsage");
const cpuCtx = cpuUsage.getContext('2d');
const threePIByTwo = (Math.PI);
const activeColor = '#FDAC42',
    inactiveColor = '#282A2D';
const memUsage = document.getElementById("memoryUsageChart");
const memCtx = memUsage.getContext('2d');
var cpuX = 0;
var cpuY = 0;
var memX = 0;
var memY = 0;
var cpuResult = 0;
var memResult = 0;
var cpuUsageLabel = document.getElementById("cpuUsageLblId");
var memUsageLabel = document.getElementById("memoryUsageLblId");
var loops = 0;

initialize();

function setUsageValue(c, m) {
    cpuResult = (180 * c) / 100;
    memResult = (180 * m) / 100;
}

function initialize() {
    setUsageValue(viewmodel.cpu.usage, viewmodel.memory.usage);
    memUsage.height = 140;
    cpuUsage.height = 140;
    cpuX = cpuUsage.width / 2;
    cpuY = 130;
    memX = memUsage.width / 2;
    memY = 130;
    animateOnInit();
    currentDistance = 0;
    cpuUsageLabel.innerHTML = viewmodel.cpu.usage + "%";
    memUsageLabel.innerHTML = viewmodel.memory.usage + "%";
}

function animateOnInit() {
    debugger;
    if (cpuResult > memResult)
        loops = cpuResult;
    else
        loops = memResult;

    drawCPUCircle(cpuX, cpuY, 100, Math.PI, 2 * Math.PI, false, inactiveColor, 25);
    drawMemCircle(memX, memY, 100, Math.PI, 2 * Math.PI, false, inactiveColor, 25);

    for (i = 0; i <= loops; i++) {
        setDelay(i);
    }
    finished = true;
}
function setDelay(i) {
    setTimeout(function () {
        distance = i;
        if (cpuResult >= distance)
            drawCPUBar(distance);

        if (memResult >= distance)
            drawMemBar(distance);
    }, 15 * i);
}
function drawCPUBar(distance) {
    drawCPUCircle(cpuX, cpuY, 100, Math.PI, setRadians(distance) + Math.PI, false, activeColor, 25);
}

function drawMemBar(distance) {
    drawMemCircle(memX, memY, 100, Math.PI, setRadians(distance) + Math.PI, false, activeColor, 25);
}

function setRadians(deg) {
    return (Math.PI / 180) * deg;
}

function drawArcCPU(x, y, radius, start, end, clockwise) {
    cpuCtx.beginPath();
    cpuCtx.arc(x, y, radius, start, end, clockwise);
}

function drawArcMemory(x, y, radius, start, end, clockwise) {
    memCtx.beginPath();
    memCtx.arc(x, y, radius, start, end, clockwise);
}

function drawCPUCircle(x, y, radius, start, end, clockwise, color, thickness) {
    cpuCtx.strokeStyle = color;
    cpuCtx.lineWidth = thickness;
    drawArcCPU(x, y, radius, start, end, clockwise)
    cpuCtx.stroke();
}
function drawMemCircle(x, y, radius, start, end, clockwise, color, thickness) {
    memCtx.strokeStyle = color;
    memCtx.lineWidth = thickness;
    drawArcMemory(x, y, radius, start, end, clockwise)
    memCtx.stroke();
}