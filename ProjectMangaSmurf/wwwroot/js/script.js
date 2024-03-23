
/*======================= ANIMATION SEARCH BAR ==========================*/
const icon = document.querySelector(".icon");
const search = document.querySelector(".search");

icon.onclick = () => {
    search.classList.toggle('active');
}
const clear = document.querySelector(".clear");
const valueTruyen = document.getElementById('search-truyen');
clear.onclick = () => {
    if (valueTruyen.value == '') {
        search.classList.remove('active');
    }
    else {
        valueTruyen.value = '';
    }
}



/* ======================== active rank ================================ */
var header = document.getElementById("rank__type");
var btns = header.getElementsByClassName("nav__item");
for (var i = 0; i < btns.length; i++) {
    btns[i].addEventListener("click", function () {
        var current = document.getElementsByClassName("active-rank");
        if (current.length > 0) {
            current[0].className = current[0].className.replace(" active-rank", "");
        }
        this.className += " active-rank";
    });
}

/*======================== FUNCTION UI ĐÁNH GIÁ ============================*/
const stars = document.querySelectorAll(".rating i");
stars.forEach((star, index1) => {
    star.addEventListener("click", () => {
        stars.forEach((star, index2) => {
            index1 >= index2 ? star.classList.add("active") : star.classList.remove("active");
        });
    });
});
