import React from 'react';
import type { Picture } from '../types/Picture';

interface CosmoCardProps {
  cosmo: Picture;
}

export const CosmoCard: React.FC<CosmoCardProps> = ({ cosmo }) => {
  const cardStyle: React.CSSProperties = {
    width: 240,
    borderRadius: 12,
    overflow: 'hidden',
    boxShadow: '0 4px 8px rgba(0,0,0,0.08)',
    backgroundColor: '#ffffff',
    textAlign: 'center',
    transition: 'transform 0.2s ease, box-shadow 0.2s ease',
  };

  const imgStyle: React.CSSProperties = {
    width: '100%',
    height: 200,
    objectFit: 'cover',
    borderBottom: '1px solid #e5e7eb',
  };

  const infoStyle: React.CSSProperties = {
    padding: '0.75rem 1rem 1rem',
  };

  const nameStyle: React.CSSProperties = {
    fontSize: '1.05rem',
    fontWeight: 600,
    color: '#1f2937',
    marginBottom: '0.4rem',
  };

  const priceStyle: React.CSSProperties = {
    fontSize: '0.95rem',
    color: '#374151',
    fontWeight: 500,
    background: '#f3f4f6',
    borderRadius: 8,
    display: 'inline-block',
    padding: '0.25rem 0.75rem',
  };

  const hoverStyle: React.CSSProperties = {
    transform: 'translateY(-4px)',
    boxShadow: '0 8px 16px rgba(0,0,0,0.12)',
  };

  const [hovered, setHovered] = React.useState(false);

  return (
    <div
      style={hovered ? { ...cardStyle, ...hoverStyle } : cardStyle}
      onMouseEnter={() => setHovered(true)}
      onMouseLeave={() => setHovered(false)}
    >
      <img src={cosmo.s3Url} alt={cosmo.pictureName} style={imgStyle} />
      <div style={infoStyle}>
        <div style={nameStyle}>{cosmo.pictureName}</div>
        <div style={priceStyle}>
          ${cosmo.price ? cosmo.price.toFixed(2) : '—'}
        </div>
      </div>
    </div>
  );
};
