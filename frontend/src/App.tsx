import { useEffect, useState } from 'react';
import './App.css';

interface Picture {
  Id: string;
  Url: string;
  Description: string;
}

export default function App() {
  const [pictures, setPictures] = useState<Picture[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState('');

  const API_URL =
    'https://flwg9mklwg.execute-api.us-west-2.amazonaws.com/api/pictures';

  useEffect(() => {
    fetch(API_URL)
      .then((res) => {
        if (!res.ok) throw new Error('Failed to fetch pictures.');
        return res.json();
      })
      .then((data) => {
        setPictures(data);
        setLoading(false);
      })
      .catch(() => {
        setError('Unable to load pictures at this time.');
        setLoading(false);
      });
  }, []);

  return (
    <div className="container">
      <header className="header">
        <h1 className="title">Cosmo Image Gallery</h1>
        <p className="subtitle">
          Powered by AWS Lambda · DynamoDB · S3 · API Gateway
        </p>
      </header>

      {loading && <div className="loader"></div>}

      {error && <div className="error">{error}</div>}

      <div className="gallery">
        {pictures.map((pic) => (
          <div key={pic.Id} className="card">
            <img src={pic.Url} alt={pic.Id} className="card-image" />
            <div className="card-body">
              <h3 className="card-title">{pic.Id.replace('.jpg', '')}</h3>
              <p className="card-text">
                {pic.Description || 'No description available'}
              </p>
            </div>
          </div>
        ))}
      </div>
    </div>
  );
}
