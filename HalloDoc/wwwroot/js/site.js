var phContact = document.querySelectorAll('.iti--allow-dropdown');
phContact.forEach(item => {
    item.classList.add('h-75');
})

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

