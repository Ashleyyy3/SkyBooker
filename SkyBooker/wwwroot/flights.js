const searchBtn = document.getElementById("searchBtn");

if (searchBtn) {
    searchBtn.addEventListener("click", function () {
        const from = document.getElementById("from").value;
        const to = document.getElementById("to").value;
        const date = document.getElementById("date").value;

        const params = new URLSearchParams();

        if (from) params.append("from", from);
        if (to) params.append("to", to);
        if (date) params.append("date", date);

        window.location.href = "flights.html?" + params.toString();
    });
}

//API call to get flights based on search criteria

const flightsContainer = document.getElementById("flightsContainer");

if (flightsContainer) {
    loadFlights();
}

async function loadFlights() {
    const queryString = window.location.search;

    const response = await fetch("/api/Flights/search" + queryString);
    const flights = await response.json();

    flightsContainer.innerHTML = "";

    if (flights.length === 0) {
        flightsContainer.innerHTML = "<p>No flights found.</p>";
        return;
    }

    flights.forEach(flight => {
        const card = document.createElement("div");
        card.className = "flight-card";

        card.innerHTML = `
            <div>
                <h2>${flight.from} → ${flight.to}</h2>
                <div class="flight-info">
                    <p>Departure: ${new Date(flight.departureTime).toLocaleString()}</p>
                    <p>Price: ${flight.price} SEK</p>
                    <p>Seats left: ${flight.availableSeats}</p>
                </div>
            </div>

            <button>Book flight</button>
        `;

        flightsContainer.appendChild(card);
    });
}