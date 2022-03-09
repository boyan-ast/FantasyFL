var textAreaErrorElement = document.getElementById('error-message');

if (textAreaErrorElement.textContent) {
    let mainElement = document.querySelector('main');
    mainElement.onLoad = swal(textAreaErrorElement.textContent);
}