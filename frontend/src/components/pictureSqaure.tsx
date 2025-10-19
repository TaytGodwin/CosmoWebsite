import React from 'react';
import type { Cosmo } from '../types/cosmo';

interface CosmoCardProps {
  cosmo: Cosmo;
}

export const CosmoCard: React.FC<CosmoCardProps> = ({ cosmo }) => {
  return (
    <div style={{ textAlign: 'center' }}>
      <img
        src={cosmo.imageUrl}
        alt={cosmo.pictureName}
        style={{ width: '200px', height: '200px', objectFit: 'cover' }}
      />
      <p style={{ marginTop: '8px', fontWeight: 600 }}>{cosmo.pictureName}</p>
    </div>
  );
};
