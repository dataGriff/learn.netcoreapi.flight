const api_base_url = "http://localhost:3000/api/v1/flightplan/";

let username = "admin";
let password = "P@ssw0rd";

async function get_all_flight_plans() {
    let api_response = await fetch(api_base_url, {
        method: "GET",
        headers: {
            'Authorization': 'Basic ' + btoa(username + ":" + password)
        }
    });

    if(api_response.status == 204) {
        let html = 'No flight plans found. Add a flight plan by entering data in the flight plan form.';
        document.getElementById('flight_plan_list').innerHTML = html;
        return;
    }

    let api_data = await api_response.json();
    api_data.forEach(flight_plan => {
        html += '<div class="card mb-5">' +
            '       <div class="card-header container-fluid">' +
            '           <div class="row">' +
            '              <div class="col-md-10">' +
            '                   <h5>' + flight_plan.flightType + ' flight plan from ' + flight_plan.departingAirport + ' to ' + flight_plan.arrival_airport + '</h5>' +
            '               </div>' +
            '               <div class="col-md-2 float-right">' +
            '                  <button class="btn btn-danger" style="margin-left: 1em" ' +
            '                   onclick="delete_flight_plan(\'' + flight_plan.flightPlanId + '\').then(get_all_flight_plans)">Delete</button>' +
            '              </div>' +
            '           </div>' +
            '    </div>' +
            '    <div class="card-body">' +
            '        <div class="row m-3 mb-4">' +
            '            <div class="col-3">' +
            '                <h6>Flight Type:</h6> '+ flight_plan.flightType +
            '            </div>' +
            '            <div class="col-3">' +
            '                <h6>Aircraft Identification:</h6>' + flight_plan.aircraftIdentification +
            '            </div>' +
            '            <div class="col-3">' +
            '                <h6>Aircraft Type:</h6> ' + flight_plan.aircraftType +
            '            </div>' +
            '            <div class="col-3">' +
            '                <h6>Fuel on Board:</h6>' + flight_plan.fuelHours + ' hours ' + flight_plan.fuelMinutes +
            '            </div>' +
            '        </div>' +
            '        <div class="row m-3 mb-4">' +
            '            <div class="col-3">' +
            '                <h6>Filed Altitude:</h6> ' + flight_plan.altitude + ' feet' +
            '            </div>' +
            '            <div class="col-3">' +
            '                <h6>Filed Airspeed:</h6> ' + flight_plan.airspeed + ' knots' +
            '            </div>' +
            '            <div class="col-3">' +
            '                <h6>Departure Time:</h6>' + new Date(flight_plan.departureTime).toLocaleString()  +
            '            </div>' +
            '            <div class="col-3">' +
            '                <h6>Estimated Arrival Time:</h6>' + new Date(flight_plan.arrivalTime).toLocaleString()  +
            '            </div>' +
            '        </div>' +
            '        <div class="row m-3 mb-4">' +
            '            <div class="col-12"><h6>Route: </h6>' + flight_plan.route + '</div>' +
            '        </div>' +
            '        <div class="row m-3 mb-4">' +
            '            <div class="col-12"><h6>Remarks: </h6>'+ flight_plan.remarks + '   </div>' +
            '        </div>' +
            '    </div>' +
            '    <div class="card-footer text-muted"> ' +
            '    Flight Plan Id: ' +   flight_plan.flightPlanId   +
            '   </div>' +
            '</div>'

         document.getElementById('flight_plan_list').innerHTML = html;
    })


}

async function delete_flight_plan(flightPlanId) {
    let response = await fetch(api_base_url + flightPlanId, {
        method: 'DELETE',
        headers: {
            'Authorization': 'Basic ' + btoa(username + ":" + password)
        }
    });

    if(response.status !== 200) {
        alert(response.status);
    }
}

async function load_flight_plan() {
    let flightPlanId = document.getElementById("flightPlanId").value;
    let api_response = await fetch(api_base_url + flightPlanId, {
        method: 'GET',
        headers: {
            'Authorization': 'Basic ' + btoa(username + ":" + password)
        }
    });

    if(api_response.status !== 200) {
        alert(api_response.status);
        return;
    }

    let api_data = await api_response.json();

    document.getElementById('tailNumber').value = api_data.aircraftIdentification;
    document.getElementById('aircraftType').value = api_data.aircraftType;
    document.getElementById('airspeed').value = api_data.airspeed;
    document.getElementById('altitude').value = api_data.altitude;
    document.getElementById('paxOnBoard').value = api_data.number_onboard;
    document.getElementById('departureTime').value = api_data.departureTime.substring(0, api_data.departureTime.length - 1);
    document.getElementById('arrivalTime').value = api_data.estimatedArrivalTime.substring(0, api_data.estimatedArrivalTime.length - 1);
    document.getElementById('departAirport').value = api_data.departingAirport;
    document.getElementById('arriveAirport').value = api_data.arrival_airport;
    document.getElementById('route').value = api_data.route;
    document.getElementById('remarks').value = api_data.remarks;
    document.getElementById('fuelHours').value = api_data.fuelHours;
    document.getElementById('fuelMinutes').value = api_data.fuelMinutes;
    document.getElementById('paxOnBoard').value = api_data.number_onboard;

    if (api_data.flightType === 'VFR') {
        document.getElementById('inlineVFR').checked = true;
    }
    else if (api_data.flightType === 'IFR') {
        document.getElementById('inlineIFR').checked = true;
    }

    document.getElementById('fileButton').hidden = true;
    document.getElementById('updateButton').hidden = false;
}

async function update_flight_plan() {
    let selected_flightType = 'Unknown';
    if(document.getElementById('inlineVFR').checked) {
        selected_flightType = 'VFR';
    }
    else if(document.getElementById('inlineVFR').checked) {
        selected_flightType = 'IFR';
    }

    let api_response = await fetch(api_base_url, {
        method: "PUT",
        headers: {
            'Authorization': 'Basic ' + btoa(username + ":" + password),
            'Content-Type': 'application/json'
        },
        body: JSON.stringify({
            flightPlanId: document.getElementById("flightPlanId").value,
            altitude: Number(document.getElementById('altitude').value),
            aircraftIdentification: document.getElementById('tailNumber').value,
            aircraftType: document.getElementById('aircraftType').value,
            airspeed: Number(document.getElementById('airspeed').value),
            arrival_airport: document.getElementById('arriveAirport').value,
            flightType: selected_flightType,
            departingAirport: document.getElementById('departAirport').value,
            departureTime: new Date(document.getElementById('departureTime').value).toISOString(),
            estimatedArrivalTime: new Date(document.getElementById('arrivalTime').value).toISOString(),
            route: document.getElementById('route').value,
            remarks: document.getElementById('remarks').value,
            fuelHours: Number(document.getElementById('fuelHours').value),
            fuelMinutes: Number(document.getElementById('fuelMinutes').value),
            number_onboard: Number(document.getElementById('paxOnBoard').value)
        })
    });

    if(api_response.status !== 200) {
        alert(api_response.status);
    }
}

async function file_flight_plan() {
    let selected_flightType = 'Unknown';
    if(document.getElementById('inlineVFR').checked) {
        selected_flightType = 'VFR';
    }
    else if(document.getElementById('inlineVFR').checked) {
        selected_flightType = 'IFR';
    }

    let api_response = await fetch(api_base_url + 'file', {
        method: 'POST',
        headers: {
            'Authorization': 'Basic ' + btoa(username + ":" + password),
            'Content-Type': 'application/json'
        },
        body: JSON.stringify({
            flightPlanId: document.getElementById("flightPlanId").value,
            altitude: Number(document.getElementById('altitude').value),
            aircraftIdentification: document.getElementById('tailNumber').value,
            aircraftType: document.getElementById('aircraftType').value,
            airspeed: Number(document.getElementById('airspeed').value),
            arrival_airport: document.getElementById('arriveAirport').value,
            flightType: selected_flightType,
            departingAirport: document.getElementById('departAirport').value,
            departureTime: new Date(document.getElementById('departureTime').value).toISOString(),
            estimatedArrivalTime: new Date(document.getElementById('arrivalTime').value).toISOString(),
            route: document.getElementById('route').value,
            remarks: document.getElementById('remarks').value,
            fuelHours: Number(document.getElementById('fuelHours').value),
            fuelMinutes: Number(document.getElementById('fuelMinutes').value),
            number_onboard: Number(document.getElementById('paxOnBoard').value)
        })
    });

    if(api_response.status !== 200) {
        alert(api_response.status);
    }
}