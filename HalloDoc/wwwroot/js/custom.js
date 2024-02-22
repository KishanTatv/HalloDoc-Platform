document.addEventListener("DOMContentLoaded", function () {
    var LoadModal = new bootstrap.Modal(document.getElementById('LoadModal'));
    LoadModal.show();

    var LoadDone = document.getElementById("LoadDone");
    LoadDone.addEventListener("click", (event) => {
        LoadModal.hide();
        event.preventDefault();
    });
});
