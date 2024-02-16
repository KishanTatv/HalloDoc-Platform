var LoadModal = new bootstrap.Modal(document.getElementById('LoadModal'), {})
LoadModal.show();

var LoadDone = document.getElementById("LoadDone");
LoadDone.addEventListener("click", ()=>{
    LoadModal.hide();
    event.preventDefault();
});