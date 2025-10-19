import './App.css';
import { CosmoCard } from './components/pictureSqaure';
import type { Cosmo } from './types/cosmo';

function App() {
  // Example data — replace with data fetched from your backend later
  const cosmos: Cosmo[] = [
    {
      id: 1,
      pictureName: 'Caramel Glazed Doughnut',
      imageUrl: 'https://your-backend/api/cosmos/1/image',
    },
    {
      id: 2,
      pictureName: 'Chocolate Sprinkle Doughnut',
      imageUrl: 'https://your-backend/api/cosmos/2/image',
    },
    {
      id: 3,
      pictureName: 'Strawberry Frosted Doughnut',
      imageUrl: 'https://your-backend/api/cosmos/3/image',
    },
    {
      id: 4,
      pictureName: 'Blueberry Filled Doughnut',
      imageUrl: 'https://your-backend/api/cosmos/4/image',
    },
    {
      id: 5,
      pictureName: 'Vanilla Glazed Doughnut',
      imageUrl: 'https://your-backend/api/cosmos/5/image',
    },
    {
      id: 6,
      pictureName: 'Sugar Ring Doughnut',
      imageUrl: 'https://your-backend/api/cosmos/6/image',
    },
  ];

  return (
    <div style={{ textAlign: 'center' }}>
      <h1>Pictures of Cosmo</h1>
      <p>Buy framed pictures nationwide.</p>

      {/* Grid container */}
      <div className="cosmo-grid">
        {cosmos.map((c) => (
          <CosmoCard key={c.id} cosmo={c} />
        ))}
      </div>
    </div>
  );
}

export default App;
