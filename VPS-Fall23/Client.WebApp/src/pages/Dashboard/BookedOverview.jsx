import { Fragment, useEffect } from 'react';
import { Progress, Card, Typography } from 'antd';
import useParkingZoneService from '@/services/parkingZoneOwnerService.js';
import { useState } from 'react';
import {
  Chart, CategoryScale, LinearScale, PointElement, LineElement, Title, Tooltip, Legend,
} from 'chart.js';
import { Line } from 'react-chartjs-2';

const { Text } = Typography;

function BookedOverview({ parkingZoneName }) {
  Chart.register(CategoryScale, LinearScale, PointElement, LineElement, Title, Tooltip, Legend);

  const parkingZoneService = useParkingZoneService();

  const [bookedData, setBookedData] = useState({
    doneCheckInOut: 0,
    notCheckIn: 0,
    notCheckOut: 0,
    total: 0,
    hourCash: 0,
    dayCash: 0,
    weekCash: 0,
    monthCash: 0,
    yearCash: 0,
  });

  const [chartData, setChartData] = useState({
    labels: ['1h', '1day', '1W', '1M', '1Y'],
    datasets: [
      {
        label: 'Income',
        data: [0, 0, 0, 0, 0, 0],
        borderColor: 'rgba(75, 192, 192, 1)',
        fill: false,
      },
    ],
  });

  useEffect(() => {
    console.log(parkingZoneName)
    getBookedData();
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [parkingZoneName]);

  const getBookedData = async () => {
    await parkingZoneService
      .getBookedOverview({ parkingZoneName })
      .then((res) => {
        setBookedData(res.data);
        const chart = {
          datasets: [
            {
              label: 'Daily Income',
              data: [res.data.hourCash, res.data.dayCash, res.data.weekCash, res.data.monthCash, res.data.yearCash],
              borderColor: 'rgba(75, 192, 192, 1)',
              fill: false,
            },
          ],
        };
        // console.log(chart);
        console.log(res.data);

        setChartData(chart);
      })
      .catch((error) => {
        console.error('Error fetching data:', error);
      });
  };

  const options = {
    scales: {
      x: {
        type: 'category',
        labels: ['1h', '1day', '1W', '1M', '1Y'],
      },
      y: {
        beginAtZero: true,
      },
    },
    plugins: {
      tooltip: {
        enabled: true,
        mode: 'index',
        intersect: false,
        callbacks: {
          label: (context) => {
            const label = context.dataset.label || '';
            if (label) {
              return `${label}: ${context.parsed.y.toLocaleString('vi-VN', {
                style: 'currency',
                currency: 'VND',
              })}`;
            }
            return null;
          },
        },
      },

    }
  };

  return (
    <Fragment>
      {(bookedData !== null || bookedData !== undefined) && (
        <div className="block">
          <div className="flex">
            <Card title="Booked (tháng)" bordered={true} style={{ width: 200 }}>
              <p>Tổng vé xe: {bookedData.doneCheckInOut}</p>
              <Text strong>
                Doanh thu:{' '}
                {bookedData.monthCash.toLocaleString('vi-VN', {
                  style: 'currency',
                  currency: 'VND',
                })}
              </Text>
            </Card>
            <Card title="Đã hoàn thành" className="ml-5" bordered={true} style={{ width: 200 }}>
              <div className="flex justify-between items-center">
                {bookedData.doneCheckInOut}
                <Progress
                  type="circle"
                  percent={((bookedData.doneCheckInOut / bookedData.total) * 100).toFixed(1)}
                  size={60}
                />
              </div>
            </Card>
            <Card title="Chưa Check In" className="ml-5" bordered={true} style={{ width: 200 }}>
              <div className="flex justify-between items-center">
                {bookedData.notCheckIn}
                <Progress type="circle" percent={((bookedData.notCheckIn / bookedData.total) * 100).toFixed(1)} size={60} />
              </div>
            </Card>
            <Card title="Chưa Check Out" className="ml-5" bordered={true} style={{ width: 200 }}>
              <div className="flex justify-between items-center">
                {bookedData.notCheckOut}
                <Progress type="circle" percent={((bookedData.notCheckOut / bookedData.total) * 100).toFixed(1)} size={60} />
              </div>
            </Card>
          </div>
          <div className="mt-5">
            <Line data={chartData} options={options} />
          </div>
        </div>
      )
      }
    </Fragment >
  );
}

export default BookedOverview;
