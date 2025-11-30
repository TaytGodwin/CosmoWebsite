import { useEffect, useState } from 'react';

type Picture = {
  Id: string;
  Url: string;
  Description: string;
  Price: number;
};

const API_URL =
  'https://flwg9mklwg.execute-api.us-west-2.amazonaws.com/api/pictures';

function getRandomPrice() {
  const min = 7.99;
  const max = 29.99;
  return Number((Math.random() * (max - min) + min).toFixed(2));
}

export default function Gallery() {
  const [pictures, setPictures] = useState<Picture[]>([]);

  useEffect(() => {
    fetch(API_URL)
      .then((res) => res.json())
      .then((data) => {
        // Add random price to each picture
        const priced = data.map((pic: any) => ({
          ...pic,
          Price: getRandomPrice(),
        }));
        setPictures(priced);
      })
      .catch((err) => console.error('Error fetching:', err));
  }, []);

  return (
    <div className="max-w-3xl mx-auto py-10">
      <h1 className="text-5xl font-bold text-center">Cosmo Picture Store</h1>

      <div className="mt-10 flex flex-col gap-8">
        {pictures.map((pic) => (
          <div
            key={pic.Id}
            className="bg-white shadow-lg rounded-xl overflow-hidden"
          >
            <img
              src={pic.Url}
              alt="Cosmo"
              className="w-full object-cover h-64"
            />

            <div className="p-6">
              <p className="text-xl font-semibold text-gray-900">
                ${pic.Price.toFixed(2)}
              </p>

              <p className="text-gray-600 mt-2">
                {pic.Description
                  ? pic.Description
                  : 'A high-quality Cosmo photo'}
              </p>

              <button className="mt-4 px-4 py-2 bg-blue-600 text-white rounded-lg hover:bg-blue-700">
                Buy Now
              </button>
            </div>
          </div>
        ))}
      </div>
    </div>
  );
}
