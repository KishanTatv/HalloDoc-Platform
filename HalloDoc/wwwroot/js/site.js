// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

document.getElementById('FileInput').addEventListener('change', function () {
    document.getElementById('selectedFile').value = this.files[0].name;
})

function isPasswordSame() {
    var password = document.getElementById('Password').value;
    var confirmPassword = document.getElementById('confirmPassword').value;

    if (password == confirmPassword) {
        return true;
    }
    else {
        $("#errorModal").modal("show");
        return false;
    }
}

