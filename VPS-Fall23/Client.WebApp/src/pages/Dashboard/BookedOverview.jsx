import { Fragment, useEffect } from 'react';
import { Progress, Card, Typography } from 'antd';
import useParkingZoneService from '@/services/parkingZoneOwnerService.js';
import { useState } from 'react';
import { Chart, CategoryScale, LinearScale, PointElement, LineElement, Title, Tooltip, Legend } from 'chart.js';
import { Line } from 'react-chartjs-2';

const { Text } = Typography;

function BookedOverview({ parkingZoneId }) {
  Chart.register(CategoryScale, LinearScale, PointElement, LineElement, Title, Tooltip, Legend);

  const parkingZoneService = useParkingZoneService();

  const [bookedData, setBookedData] = useState({
    booked: 0,
    deposit: 0,
    unPay: 0,
    payed: 0,
    userCancel: 0,
    parkingCancel: 0,
    payedFailed: 0,
    total: 0,
    hourCash: 0,
    dayCash: 0,
    weekCash: 0,
    monthCash: 0,
    yearCash: 0,
  });

  const [chartData, setChartData] = useState({
    labels: ['1W', '1M', '1Y', 'All'],
    datasets: [
      {
        label: 'Thống kê',
        data: [0, 0, 0, 0, 0, 0],
        borderColor: 'rgba(75, 192, 192, 1)',
        fill: false,
      },
    ],
  });

  useEffect(() => {
    getBookedData();
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [parkingZoneId]);

  const getBookedData = async () => {
    await parkingZoneService
      .getBookedOverview({ parkingZoneId })
      .then((res) => {
        setBookedData(res.data);
        const chart = {
          datasets: [
            {
              label: 'Đã Đặt Lịch',
              data: [res.data.bookedWeek, res.data.bookedMonth, res.data.bookedYear, res.data.booked],
              borderColor: 'rgba(75, 192, 192, 1)',
              fill: false,
            },
            {
              label: 'Đã Đặt Cọc',
              data: [res.data.depositWeek, res.data.depositMonth, res.data.depositYear, res.data.deposit],
              borderColor: 'rgb(155, 184, 205)',
              fill: false,
            },
            {
              label: 'Chưa Trả Tiền',
              data: [res.data.unPayWeek, res.data.unPayMonth, res.data.unPayYear, res.data.unPay],
              borderColor: 'rgb(255, 247, 212)',
              fill: false,
            },
            {
              label: 'Đã Trả Tiền',
              data: [res.data.payedWeek, res.data.payedMonth, res.data.payedYear, res.data.payed],
              borderColor: 'rgb(238, 199, 89)',
              fill: false,
            },
            {
              label: 'Người dùng hủy',
              data: [res.data.userCancelWeek, res.data.userCancelMonth, res.data.userCancelYear, res.data.userCancel],
              borderColor: 'rgb(177, 195, 129)',
              fill: false,
            },
            {
              label: 'Nhà xe hủy',
              data: [res.data.parkingCancelWeek, res.data.parkingCancelMonth, res.data.parkingCancelYear, res.data.parkingCancel],
              borderColor: 'rgb(226, 110, 229)',
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
        labels: ['1W', '1M', '1Y', 'All'],
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
              return `${label}: ${context.parsed.y}`;
            }
            return null;
          },
        },
      },
    },
  };

  return (
    <Fragment>
      {(bookedData !== null || bookedData !== undefined) && (
        <div className="block">
          <div >
            <div className="flex">
              <Card title="Đã Đặt Lịch" className="ml-5" bordered={true} style={{ width: 200 }}>
                <div className="flex justify-between items-center">
                  {bookedData.booked}
                  <Progress
                    type="circle"
                    percent={((bookedData.booked / bookedData.total) * 100).toFixed(1)}
                    size={60}
                  />
                </div>
              </Card>
              <Card title="Đã Đặt Cọc" className="ml-5" bordered={true} style={{ width: 200 }}>
                <div className="flex justify-between items-center">
                  {bookedData.deposit}
                  <Progress
                    type="circle"
                    percent={((bookedData.deposit / bookedData.total) * 100).toFixed(1)}
                    size={60}
                  />
                </div>
              </Card>
              <Card title="Chưa Trả Tiền" className="ml-5" bordered={true} style={{ width: 200 }}>
                <div className="flex justify-between items-center">
                  {bookedData.unPay}
                  <Progress
                    type="circle"
                    percent={((bookedData.unPay / bookedData.total) * 100).toFixed(1)}
                    size={60}
                  />
                </div>
              </Card>

            </div>
            <div className="flex">
              <Card title="Đã Trả Tiền" className="ml-5" bordered={true} style={{ width: 200 }}>
                <div className="flex justify-between items-center">
                  {bookedData.payed}
                  <Progress
                    type="circle"
                    percent={((bookedData.payed / bookedData.total) * 100).toFixed(1)}
                    size={60}
                  />
                </div>
              </Card>
              <Card title="Người dùng hủy" className="ml-5" bordered={true} style={{ width: 200 }}>
                <div className="flex justify-between items-center">
                  {bookedData.userCancel}
                  <Progress
                    type="circle"
                    percent={((bookedData.userCancel / bookedData.total) * 100).toFixed(1)}
                    size={60}
                  />
                </div>
              </Card>
              <Card title="Nhà xe hủy" className="ml-5" bordered={true} style={{ width: 200 }}>
                <div className="flex justify-between items-center">
                  {bookedData.parkingCancel}
                  <Progress
                    type="circle"
                    percent={((bookedData.parkingCancel / bookedData.total) * 100).toFixed(1)}
                    size={60}
                  />
                </div>
              </Card>
            </div>

          </div>

          <div className="mt-5">
            <Line data={chartData} options={options} />
          </div>
        </div>
      )}
    </Fragment>
  );
}

export default BookedOverview;
