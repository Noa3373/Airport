
const airportContainer = document.getElementById('container');
const flightBoard = document.getElementById('flightBoard').getElementsByTagName('tbody')[0];
const connection = new signalR.HubConnectionBuilder()
    .withUrl("/flightHub")
    .build();
var airplaneImage;
var flightBoardRow;

connection.start().then(() => {
    console.log("SignalR Connected!");
}).catch(err => {
    console.error("SignalR Connection Error: ", err);
});

connection.on("ReceiveFlight", (flight) => {
    console.log("Received Flight:", flight);
    airplaneImage1 = document.getElementById(`img${flight.airplane.flightNumber}`);
    if (flight.terminal.number == 1 || airplaneImage1 == undefined) {
        createImgElement(flight)
        setImgPos(flight)
        airportContainer.appendChild(airplaneImage);
        setFlightBoard(flight)
    }
    else {
        airplaneImage = document.getElementById(`img${flight.airplane.flightNumber}`);
        setImgPos(flight);
        updateFlightBoard(flight);
        if (flight.terminal.number == 5)
            setImgtransform(-1);
        else if (flight.terminal.number == 8)
            setImgtransform(1);
        else if (flight.terminal.number == 9) {
            removeImg(flight);
            removeRowFromFlightBoard(flight)
        }
    }
    
});

function createImgElement(flight) {
    airplaneImage = document.createElement('img');
    airplaneImage.src = flight.airplane.imgPath;
    airplaneImage.id = `img${flight.airplane.flightNumber}`;
}
function setImgPos(flight) {
    airplaneImage.style.transition = "left 0.5s ease-in-out, top 0.5s ease-in-out";
    airplaneImage.style.left = flight.terminal.positionX + 'px';
    airplaneImage.style.top = flight.terminal.positionY + 'px';
}

function setImgtransform(number) {
    airplaneImage.style.transform = `scaleX(${number})`;
}

function removeImg(flight) {
    let img = airplaneImage;
    setTimeout(() => {
        img.style.left = (flight.terminal.positionX - 300) + 'px';
        img.style.opacity = '0';
        img.style.transition = "left 2s ease-in-out, opacity 2s ease-in-out";
    }, 1000);

    setTimeout(() => {
        airportContainer.removeChild(img);
    }, 3000);
}

function setFlightBoard(flight) {
    flightBoardRow = flightBoard.insertRow();
    flightBoardRow.id = `row${flight.airplane.flightNumber}`;
    let flightNumber = flightBoardRow.insertCell(0);
    let terminal = flightBoardRow.insertCell(1);

    flightNumber.textContent = flight.airplane.flightNumber;
    terminal.textContent = flight.terminal.number;
}

function updateFlightBoard(flight) {
    flightBoardRow = document.getElementById(`row${flight.airplane.flightNumber}`);
    let cells = flightBoardRow.getElementsByTagName('td');
    cells[1].textContent = flight.terminal.number;
}

function removeRowFromFlightBoard(flight) {
    let row = flightBoardRow
    setTimeout(() => {
        flightBoard.removeChild(row);
    }, 3000);
}