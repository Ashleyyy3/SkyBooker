// ── Index page: handle search button ──
const searchBtn = document.getElementById("searchBtn");
if (searchBtn) {
    searchBtn.addEventListener("click", function () {
        const from = document.getElementById("from").value.trim();
        const to = document.getElementById("to").value.trim();
        const date = document.getElementById("date").value;
        if (!from || !to || !date) {
            alert("Please fill in all fields before searching.");
            return;
        }
        const params = new URLSearchParams({ from, to, date });
        window.location.href = "flights.html?" + params.toString();
    });
}

// ── Flights page: load and display results ──
const flightsContainer = document.getElementById("flightsContainer");
if (flightsContainer) {
    loadFlights();
}

async function loadFlights() {
    const params = new URLSearchParams(window.location.search);
    const from = params.get("from");
    const to = params.get("to");
    const date = params.get("date");

    // Show search summary
    const subtitle = document.getElementById("searchSummary");
    if (subtitle && from && to && date) {
        const formatted = new Date(date).toLocaleDateString("en-GB", {
            day: "numeric", month: "long", year: "numeric"
        });
        subtitle.textContent = `${from} → ${to} · ${formatted}`;
    }

    try {
        const response = await fetch("/api/Flights/search" + window.location.search);
        const flights = await response.json();

        // Show count
        const countEl = document.getElementById("flightCount");
        if (countEl) countEl.textContent = `${flights.length} flight${flights.length !== 1 ? "s" : ""} found`;

        flightsContainer.innerHTML = "";

        if (flights.length === 0) {
            flightsContainer.innerHTML = `
                <div class="no-results">
                    <i class="bi bi-airplane"></i>
                    <p>No flights found for this route and date.</p>
                    <a href="index.html">Search again</a>
                </div>`;
            return;
        }

        flights.forEach(flight => {

        //This calculates the arravial time, duration, correct airline flight no and weather the flight is direct
            const dep = new Date(flight.departureTime);
            const arr = new Date(flight.arrivalTime);

            const depTime = dep.toLocaleTimeString("en-GB", {
                hour: "2-digit",
                minute: "2-digit"
            });

            const arrTime = arr.toLocaleTimeString("en-GB", {
                hour: "2-digit",
                minute: "2-digit"
            });

            const depDate = dep.toLocaleDateString("en-GB", {
                day: "numeric",
                month: "short",
                year: "numeric"
            });

            const flightNum =
                flight.airlineCode + String(flight.id).padStart(4, "0");

            const durationMinutes = Math.round((arr - dep) / 60000);

            const duration =
                `${Math.floor(durationMinutes / 60)}h ${durationMinutes % 60}m`;

            const stops =
                flight.stops === 0
                    ? "Direct"
                    : `${flight.stops} stop${flight.stops > 1 ? "s" : ""}`;

            const card = document.createElement("div");
            card.className = "flight-card";
            card.innerHTML = `
    <div class="flight-card-left">

        <div class="airline-row">
            <div class="airline-badge">
                ${flight.airlineCode}
            </div>

            <div>
                <strong class="airline-name">
                    ${flight.airline}
                </strong>

                <div class="flight-number">
                    ${flightNum}
                </div>
            </div>
        </div>

        <div class="comparison-route">

            <div class="route-point">
                <span class="route-time">${depTime}</span>
                <span class="airport-code">
                    ${flight.departureAirportCode}
                </span>
            </div>

            <div class="journey-summary">
                <span>${duration}</span>

                <div class="journey-line">
                    <i class="bi bi-airplane"></i>
                </div>

                <span class="${flight.stops === 0 ? "direct" : "has-stops"}">
                    ${stops}
                </span>
            </div>

            <div class="route-point">
                <span class="route-time">${arrTime}</span>
                <span class="airport-code">
                    ${flight.arrivalAirportCode}
                </span>
            </div>

        </div>

        <div class="flight-meta">
            <span>
                <i class="bi bi-calendar3"></i>
                ${depDate}
            </span>

            <span>
                ${flight.from} to ${flight.to}
            </span>
        </div>

    </div>

    <div class="flight-card-right">

        <div class="provider-label">
            Deal from ${flight.provider}
        </div>

        <div class="flight-price">
            ${flight.price}
            <span>SEK</span>
        </div>

        <a
            class="book-btn deal-btn"
            href="${flight.bookingUrl}"
            target="_blank"
            rel="noopener noreferrer"
        >
            View deal
            <i class="bi bi-box-arrow-up-right"></i>
        </a>

    </div>
   
`;

            flightsContainer.appendChild(card);
        });

    } catch (err) {
        flightsContainer.innerHTML = `<div class="no-results"><p>Something went wrong. Please try again.</p></div>`;
    }
}

// ── Booking modal ──
function openBookingModal(flightId, from, to, time, price) {
    document.getElementById("modalRoute").textContent = `${from} → ${to} · ${time}`;
    document.getElementById("modalPrice").textContent = `${price} SEK`;
    document.getElementById("modalFlightId").value = flightId;
    document.getElementById("bookingModal").classList.add("active");
}

function closeBookingModal() {
    document.getElementById("bookingModal").classList.remove("active");
    document.getElementById("bookingForm").reset();
    document.getElementById("bookingError").textContent = "";
}

const bookingForm = document.getElementById("bookingForm");
if (bookingForm) {
    bookingForm.addEventListener("submit", async function (e) {
        e.preventDefault();
        const errorEl = document.getElementById("bookingError");
        errorEl.textContent = "";

        const dto = {
            flightId: parseInt(document.getElementById("modalFlightId").value),
            passengerName: document.getElementById("passengerName").value.trim(),
            passengerEmail: document.getElementById("passengerEmail").value.trim()
        };

        try {
            const response = await fetch("/api/Bookings", {
                method: "POST",
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify(dto)
            });

            if (!response.ok) {
                const msg = await response.text();
                errorEl.textContent = msg || "Booking failed. Please try again.";
                return;
            }

            const booking = await response.json();
            const ref = "SBK-" + String(booking.id).padStart(6, "0");
            closeBookingModal();
            showConfirmation(ref, dto.passengerEmail);

        } catch (err) {
            errorEl.textContent = "Something went wrong. Please try again.";
        }
    });
}

function showConfirmation(ref, email) {
    document.getElementById("confirmRef").textContent = ref;
    document.getElementById("confirmEmail").textContent = email;
    document.getElementById("confirmationModal").classList.add("active");
}

function closeConfirmation() {
    document.getElementById("confirmationModal").classList.remove("active");
}

// ── Bookings page ──
const bookingSearchBtn = document.getElementById("bookingSearchBtn");
if (bookingSearchBtn) {
    bookingSearchBtn.addEventListener("click", searchBookings);
}

async function searchBookings() {
    const email = document.getElementById("bookingEmail").value.trim();
    const resultsEl = document.getElementById("bookingsResults");

    if (!email) {
        resultsEl.innerHTML = `<p class="error-msg">Please enter your email address.</p>`;
        return;
    }

    try {
        const response = await fetch(`/api/Bookings/by-email?email=${encodeURIComponent(email)}`);
        const bookings = await response.json();

        if (bookings.length === 0) {
            resultsEl.innerHTML = `<div class="no-results"><i class="bi bi-inbox"></i><p>No bookings found for this email.</p></div>`;
            return;
        }

        resultsEl.innerHTML = bookings.map(b => {
            const ref = "SBK-" + String(b.id).padStart(6, "0");
            const booked = new Date(b.bookingDate).toLocaleDateString("en-GB", {
                day: "numeric", month: "long", year: "numeric"
            });
            return `
                <div class="booking-card">
                    <div class="booking-ref">${ref}</div>
                    <div class="booking-info">
                        <p><strong>${b.passengerName}</strong></p>
                        <p>Flight ID: ${b.flightId}</p>
                        <p>Booked: ${booked}</p>
                    </div>
                    <button class="cancel-btn" onclick="cancelBooking(${b.id}, this)">
                        Cancel booking
                    </button>
                </div>`;
        }).join("");

    } catch (err) {
        resultsEl.innerHTML = `<p class="error-msg">Something went wrong. Please try again.</p>`;
    }
}

async function cancelBooking(id, btn) {
    if (!confirm("Are you sure you want to cancel this booking?")) return;

    try {
        const response = await fetch(`/api/Bookings/${id}`, { method: "DELETE" });
        if (response.ok) {
            btn.closest(".booking-card").remove();
        } else {
            alert("Could not cancel booking. Please try again.");
        }
    } catch {
        alert("Something went wrong.");
    }
}