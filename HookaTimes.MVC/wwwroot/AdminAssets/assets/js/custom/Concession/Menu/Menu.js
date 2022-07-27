const inputelement = document.querySelector('#flexSwitch30x50');
const labelelement = document.querySelector('#checkin');


inputelement.addEventListener('change', (event) => {
    if (event.currentTarget.checked) {
        labelelement.textContent = 'IN'
    } else {
        labelelement.textContent = 'OUT'
    }
})