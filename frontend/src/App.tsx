import './App.css';
import { CosmoCard } from './components/pictureSqaure';
import type { Picture } from './types/Picture';
import { useEffect, useState } from 'react';

function App() {
  const [pictures, setPictures] = useState<Picture[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  // Change this to your actual backend URL
  const baseUrl = import.meta.env.VITE_API_URL || 'http://localhost:5000';

  useEffect(() => {
    async function fetchPictures() {
      try {
        const response = await fetch(`${baseUrl}/api/pictures`);
        if (!response.ok) throw new Error(`HTTP error ${response.status}`);
        const data = (await response.json()) as Picture[];
        setPictures(data);
      } catch (err: any) {
        setError(err.message);
      } finally {
        setLoading(false);
      }
    }
    fetchPictures();
  }, [baseUrl]);

  if (loading) return <p>Loading pictures...</p>;
  if (error) return <p style={{ color: 'red' }}>Error: {error}</p>;

  return (
    <div style={{ textAlign: 'center' }}>
      <h1>Pictures of Cosmo</h1>
      <p>Buy framed pictures nationwide.</p>

      <div className="cosmo-grid">
        {pictures.map((p) => (
          <CosmoCard key={p.pictureId} cosmo={p} />
        ))}
      </div>
    </div>
  );
}

export default App;
