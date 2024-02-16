document.addEventListener("DOMContentLoaded", (event) => {
    let modeIcon = document.getElementById('modeIcon');
    let lightEnable = document.querySelector('.lightEnable');
    let MICON = document.querySelector(".MICON");
    let Header = document.querySelector(".Header");

    // Check the current theme state from localStorage or default to light theme
    let isDarkTheme = localStorage.getItem("Local_mode") === "1";

    // Update the UI based on the current theme state
    if (isDarkTheme) {
        MICON.classList.remove("bi-sun");
        MICON.classList.add("bi-moon");
        lightEnable.classList.add("Dark-Theme");
        if (Header) {
            Header.style.backgroundColor = "gray";
        }
    } else {
        MICON.classList.remove("bi-moon");
        MICON.classList.add("bi-sun");
        lightEnable.classList.remove("Dark-Theme");
        if (Header) {
            Header.style.backgroundColor = "";
        }
    }

    if (modeIcon) {
        modeIcon.addEventListener("click", function () {
            // Toggle the theme state
            isDarkTheme = !isDarkTheme;

            // Update the UI based on the new theme state
            if (isDarkTheme) {
                MICON.classList.remove("bi-sun");
                MICON.classList.add("bi-moon");
                lightEnable.classList.add("Dark-Theme");
                if (Header) {
                    Header.style.backgroundColor = "gray";
                }
            } else {
                MICON.classList.remove("bi-moon");
                MICON.classList.add("bi-sun");
                lightEnable.classList.remove("Dark-Theme");
                if (Header) {
                    Header.style.backgroundColor = "";
                }
            }

            // Save the current theme state to localStorage
            localStorage.setItem("Local_mode", isDarkTheme ? "1" : "0");
        })
    }
})
