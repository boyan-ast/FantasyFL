document.querySelectorAll('input[type="checkbox"]')
    .forEach(e => e.addEventListener("click", updateFormation));

document.querySelectorAll('input[type="checkbox"].goalkeeper')
    .forEach(e => e.addEventListener("click", disableOther));

function disableOther(e) {
    let goalkeepersCheckboxElements = document.querySelectorAll('input[type="checkbox"].goalkeeper');

    for (let i = 0; i < goalkeepersCheckboxElements.length; i++) {
        if (!goalkeepersCheckboxElements[i].checked) {
            goalkeepersCheckboxElements[i].disabled = e.currentTarget.checked;
        }
    }
}

function updateFormation(e) {
    let defendersCountElement = document.querySelector('span#selected-def');
    let midfieldersCountElement = document.querySelector('span#selected-mid');
    let attackersCountElement = document.querySelector('span#selected-att');

    let selectedGoalkeepers = Array.from(document.querySelectorAll('.goalkeeper')).filter(e => e.checked);

    let selectedDefenders = Array.from(document.querySelectorAll('.defender')).filter(e => e.checked);
    defendersCountElement.textContent = selectedDefenders.length;

    let selectedMidfielders = Array.from(document.querySelectorAll('.midfielder')).filter(e => e.checked);
    midfieldersCountElement.textContent = selectedMidfielders.length;

    let selectedAttackers = Array.from(document.querySelectorAll('.attacker')).filter(e => e.checked);
    attackersCountElement.textContent = selectedAttackers.length;

    let selectedPlayers =
        selectedDefenders.length + selectedMidfielders.length + selectedAttackers.length + selectedGoalkeepers.length;
    let formationElement = defendersCountElement.parentElement;

    if (selectedPlayers != 11) {
        defendersCountElement.parentElement.classList.add("text-danger");
        defendersCountElement.parentElement.classList.remove("text-light");
    } else if (selectedPlayers == 11 && formationElement.classList.contains("text-danger")) {
        defendersCountElement.parentElement.classList.remove("text-danger");
        defendersCountElement.parentElement.classList.add("text-light");
    }
}