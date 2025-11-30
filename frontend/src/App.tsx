import { useEffect, useState } from 'react';
import './App.css';

type Picture = {
  Id: string;
  Url: string;
  Description?: string;
  price?: string;
};

export default function App() {
  const [pictures, setPictures] = useState<Picture[]>([]);

  useEffect(() => {
    fetch('YOUR_API_URL_HERE')
      .then((res) => res.json())
      .then((data: Picture[]) => {
        const withPrices = data.map((pic: Picture) => ({
          ...pic,
          price: (Math.random() * 20 + 10).toFixed(2),
        }));
        setPictures(withPrices);
      });
  }, []);

  return (
    <div>
      <h1 className="store-title">Cosmo Picture Store</h1>

      <div className="gallery-container">
        {pictures.map((pic) => (
          <div key={pic.Id} className="picture-card">
            <img src={pic.Url} alt="Cosmo" />

            <div className="price-tag">${pic.price}</div>

            <div className="description">A high-quality Cosmo photo</div>

            <a className="buy-button" href="#">
              Buy Now
            </a>
          </div>
        ))}
      </div>
    </div>
  );
}
