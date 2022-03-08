function clickHandler(e) {
    let trElement = e.currentTarget;

    let playerId = trElement.querySelector("td.playerId").textContent;
    let playerName = trElement.querySelector("td.playerName").textContent;

    console.log(playerId);
    console.log(playerName);

    let inputPlayersNameElements = document.querySelectorAll('#pickPlayersForm .playerName input');
    let inputPlayersIdElements = document.querySelectorAll('#pickPlayersForm .playerId input');

    for (let i = 0; i < inputPlayersNameElements.length; i++) {
        console.log(inputPlayersNameElements[i].value);

        if (!inputPlayersNameElements[i].value) {
            inputPlayersNameElements[i].value = playerName;

            inputPlayersIdElements[i].value = playerId;


            break;
        }

        if (i == inputPlayersNameElements.length - 1) {
            alert("Your list is full. Remove player first.");
        }
    }
}

document.querySelectorAll('#playersTable tr')
    .forEach(e => e.addEventListener("click", clickHandler));