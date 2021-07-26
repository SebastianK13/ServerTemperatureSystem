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

window.addEventListener("DOMContentLoaded", function(){
    generalState.innerHTML = "Normal";
    coreCriticalTemp.innerHTML = viewmodel.cpu.cores[0].criticalTemp+"°C";
    coresAmount.innerHTML = viewmodel.cpu.cores.length;
    moboCriticalTemp.innerHTML = viewmodel.mobo.criticalTemp+"°C";
    cpuCriticalTemp.innerHTML = viewmodel.cpu.criticalTemp+"°C";
    memTotal.innerHTML = viewmodel.memory.total;
    //FetchData();
});

function FetchData()
{
    var url = '/Monitor/CurrentReadings/'
    fetch(url, additionalParams)
    .then(function(response) {
        return response.json();
    }).then(function(data){
        console.log(data);
    }).catch(function(){
        console.log("error");
    });
}