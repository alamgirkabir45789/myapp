var imageInputFile = document.getElementById("filePath");

var wrapper = document.querySelector(".wrapper-two");
var fileName = document.querySelector(".file-name");
var defaultBtn = document.querySelector("#default-btn");
var customBtn = document.querySelector("#custom-choose-file-btn");
var cancelBtn = document.querySelector("#cancel-btn i");

if (imageInputFile === null) {
console.log("I am  response",imageInputFile)
    var imageTagCreate = document.createElement("img");
    imageTagCreate.classList.add("image-preview");
    document.querySelector(".image").append(imageTagCreate);
} else {
    var imageFileName = imageInputFile.value.split('/')[2];
    var imageTagCreate = document.createElement("img");
    var base_url = window.location.origin;
    imageTagCreate.classList.add("image-preview");
    imageTagCreate.src = `${base_url}/${imageInputFile.value}`;
    wrapper.classList.add("active");
    fileName.textContent = imageFileName;
    document.querySelector(".image").append(imageTagCreate);
    console.log("I am  response", base_url, imageTagCreate)

}



console.log("hello world")

var img = document.querySelector(".image-preview");
var regExp = /[0-9a-zA-Z\^\&\'\@@\{\}\[\]\,\$\=\!\-\#\(\)\.\%\+\~\_ ]+$/;
function defaultBtnActive() {
    defaultBtn.click();
}
defaultBtn.addEventListener("change", function () {
    var file = this.files[0];
    if (file) {
        var reader = new FileReader();
        reader.onload = function () {
            var result = reader.result;
            img.src = result;
            wrapper.classList.add("active");
        }
        cancelBtn.addEventListener("click",
            function () {
                img.src = "";
                wrapper.classList.remove("active");
            });
        reader.readAsDataURL(file);
    }
    if (this.value) {
        var valueStore = this.value.match(regExp);
        fileName.textContent = valueStore;
    }


   
});

cancelBtn.addEventListener("click",
    function () {
        img.src = "";
        wrapper.classList.remove("active");
    });