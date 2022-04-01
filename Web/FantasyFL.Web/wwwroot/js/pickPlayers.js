document.querySelectorAll('#playersTable tr')
    .forEach(e => e.addEventListener("click", clickHandler));

document.querySelectorAll('#pickPlayersForm div.playerName .remove-player')
    .forEach(e => e.addEventListener("click", removePlayerHandler));

function clickHandler(e) {
    let trElement = e.currentTarget;

    let playerId = trElement.querySelector("td.playerId").textContent;
    let playerName = trElement.querySelector("td.playerName").textContent;

    let inputPlayersNameElements = document.querySelectorAll('#pickPlayersForm .playerName input');
    let inputPlayersIdElements = document.querySelectorAll('#pickPlayersForm .playerId input');

    for (let i = 0; i < inputPlayersNameElements.length; i++) {

        if (inputPlayersNameElements[i].value == '') {
            inputPlayersNameElements[i].value = playerName;

            inputPlayersIdElements[i].value = playerId;
            trElement.remove();

            break;
        }

        if (i == inputPlayersNameElements.length - 1) {
            swal({
                text: "The list is full",
                icon: "error",
            });
        }
    }
}

function removePlayerHandler(e) {
    let buttonElement = e.currentTarget;

    let playerNameDivElement = buttonElement.parentElement.parentElement;

    let playerNameInputElement = playerNameDivElement.querySelector('div.playerName input.form-control');
    let playerIdInputElement = playerNameDivElement.nextElementSibling.querySelector('input.form-control');

    playerNameInputElement.value = '';
    playerIdInputElement.value = '';
}