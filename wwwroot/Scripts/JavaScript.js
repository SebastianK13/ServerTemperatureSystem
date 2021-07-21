var generalState = document.getElementById("gsContent");
var cpuCurrentUsage = document.getElementById("ccuUsage");
var coreCriticalTemp = document.getElementById("cstContent");
var coresAmount = document.getElementById("caContent");
var averallCpuUsage = document.getElementById("cuUsage");
var moboCriticalTemp = document.getElementById("mctContent");
var cpuCriticalTemp = document.getElementById("cctContent");
var moboCPUTemp = document.getElementById("mbcTemp");
var memTotal = document.getElementById("memtContent");
var memUsage = document.getElementById("memUsage");

window.addEventListener("DOMContentLoaded", function(){
    generalState.innerHTML = "Normal";
    coreCriticalTemp.innerHTML = viewmodel.cpu.cores[0].criticalTemp+"°C";
    coresAmount.innerHTML = viewmodel.cpu.cores.length;
    moboCriticalTemp.innerHTML = viewmodel.mb.criticalTemp+"°C";
    cpuCriticalTemp.innerHTML = viewmodel.cpu.criticalTemp+"°C";
    memTotal.innerHTML = viewmodel.memory.total;
});
