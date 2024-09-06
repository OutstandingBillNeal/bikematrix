import { useEffect, useState } from 'react';
import './App.css';

function App() {
    const [forecasts, setForecasts] = useState();
    const [bikes, setBikes] = useState();

    useEffect(() => {
        populateWeatherData();
    }, []);

    const contents = forecasts === undefined || bikes === undefined
        ? <p><em>Loading... Please refresh once the ASP.NET backend has started. See <a href="https://aka.ms/jspsintegrationreact">https://aka.ms/jspsintegrationreact</a> for more details.</em></p>
        : <div>
            <table className="table table-striped" aria-labelledby="tableLabel">
                <thead>
                    <tr>
                        <th>Date</th>
                        <th>Temp. (C)</th>
                        <th>Temp. (F)</th>
                        <th>Summary</th>
                    </tr>
                </thead>
                <tbody>
                    {forecasts.map(forecast =>
                        <tr key={forecast.date}>
                            <td>{forecast.date}</td>
                            <td>{forecast.temperatureC}</td>
                            <td>{forecast.temperatureF}</td>
                            <td>{forecast.summary}</td>
                        </tr>
                    )}
                </tbody>
            </table>
            <table className="table table-striped" aria-labelledby="tableLabel1">
                <thead>
                    <tr>
                        <th>brand</th>
                        <th>model</th>
                        <th>year</th>
                        <th>owner email</th>
                    </tr>
                </thead>
                <tbody>
                    {bikes.map(bike =>
                        <tr key={bike.id}>
                            <td>{bike.brand}</td>
                            <td>{bike.model}</td>
                            <td>{bike.year}</td>
                            <td>{bike.ownerEmail}</td>
                        </tr>
                    )}
                </tbody>
            </table>
        </div>;

    return (
        <div>
            <h1 id="tableLabel">Weather forecast</h1>
            <p>This component demonstrates fetching data from the server.</p>
            {contents}
        </div>
    );
    
    async function populateWeatherData() {
        const response = await fetch('weatherforecast');
        const data = await response.json();
        setForecasts(data);
        const bikeResponse = await fetch('bikes');
        const bikeData = await bikeResponse.json();
        setBikes(bikeData);
    }
}

export default App;