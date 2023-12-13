import React from 'react';
import { Doughnut } from 'react-chartjs-2';

const CircularChart = ({ data }) => {
  const options = {
    responsive: true,
    maintainAspectRatio: false,
  };

  return (
    <div>
      <Doughnut data={data} options={options} />
    </div>
  );
};

export default CircularChart;
