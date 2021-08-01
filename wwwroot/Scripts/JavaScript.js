var generalState = document.getElementById("gsContent");
var cpuCurrentUsage = document.getElementById("ccuUsage");
var coreCriticalTemp = document.getElementById("cstContent");
var coresAmount = document.getElementById("caContent");
var averallCpuUsage = document.getElementById("cuUsage");
var moboCriticalTemp = document.getElementById("mctContent");
var cpuCriticalTemp = document.getElementById("cctContent");
var moboCPUTemp = document.getElementById("mbcTemp");
var memTotal = document.getElementById("memtContent");
const additionalParams = {
    method:"POST"
};
var currentViewModel = viewmodel;

window.addEventListener("DOMContentLoaded", function(){
    generalState.innerHTML = "Normal";
    coreCriticalTemp.innerHTML = viewmodel.cpu.cores[0].criticalTemp+"°C";
    coresAmount.innerHTML = viewmodel.cpu.cores.length;
    moboCriticalTemp.innerHTML = viewmodel.mobo.criticalTemp+"°C";
    cpuCriticalTemp.innerHTML = viewmodel.cpu.criticalTemp+"°C";
    memTotal.innerHTML = viewmodel.memory.total;
    setInterval(FetchData, 1000);
    setInterval(UpdateCharts, 1000);
});

function FetchData()
{
    var url = '/Monitor/CurrentReadings/'
    fetch(url, additionalParams)
    .then(response => response.json())
    .then(data => {
        viewmodel = data;
    })
    .then(function(data){
        console.log(data);
    }).catch(function(){
        console.log("error");
    });

    UpdateChartUsages();
}

function UpdateCharts(){
    generateCurve();
    generateCurveT();
    updateUsageInfoSection();
    updateTempInfoSection();
}