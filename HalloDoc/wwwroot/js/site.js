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


//function SearchPatient() {
//    var input, table, tr, td, i, txtValue;
//    input = document.getElementById("InputSearch");
//    table = document.getElementById("tableD");
//    tr = table.getElementsByTagName("tr");

//    // Loop through all table rows, and hide those who don't match the search query
//    for (i = 0; i < tr.length; i++) {
//        td = tr[i].getElementsByTagName("td")[0];
//        if (td) {
//            txtValue = td.textContent || td.innerText;
//            if (txtValue.toUpperCase().indexOf(input.value.toUpperCase()) > -1) {
//                tr[i].style.display = "";
//            } else {
//                tr[i].style.display = "none";
//            }
//        }
//    }
//}
