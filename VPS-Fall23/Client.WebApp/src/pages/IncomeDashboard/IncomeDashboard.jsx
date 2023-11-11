import React, { Fragment, useState, useEffect } from 'react';
import { Card, Row, Col, Select, DatePicker, Empty, Progress, Typography } from 'antd';
import { useParams } from 'react-router-dom';
import { BarChart, Bar, XAxis, YAxis, Tooltip, Legend, CartesianGrid } from 'recharts';
import { Chart, CategoryScale, LinearScale, PointElement, LineElement, Title } from 'chart.js';
import { Line } from 'react-chartjs-2';
import styles from './IncomeDashboard.module.scss';
import { getAccountJwtModel } from '@/helpers';

import useParkingZoneService from '@/services/parkingZoneService';
import useParkingZoneOwnerService from '@/services/parkingZoneOwnerService';
import useParkingTransactionService from '@/services/parkingTransactionSerivce';

function IncomeDashboard() {

  const { Text } = Typography;


  Chart.register(CategoryScale, LinearScale, PointElement, LineElement, Title);

  const { parkingZoneName } = useParams();

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

  const parkingZoneService = useParkingZoneService();
  const parkingTransactionService = useParkingTransactionService();
  const parkingZoneOwnerService = useParkingZoneOwnerService();
  const { RangePicker } = DatePicker;
  const { Option } = Select;
  const account = getAccountJwtModel();
  const [totalIncome, setTotalIncome] = useState(0);
  const [averageMonthlyIncome, setAverageMonthlyIncome] = useState(0);
  const [averageYearlyIncome, setAverageYearlyIncome] = useState(0);
  const [dateRange, setDateRange] = useState('all');
  const [data, setData] = useState([]);
  const [ParkingZoneOptions, setParkingZoneOptions] = useState([]);
  const [selectedParkingZone, setSelectedParkingZone] = useState('');
  const [ParkingZoneData, setParkingZoneData] = useState([]);

  const handleDateRangeChange = (value) => {
    setDateRange(value);
  };

  const handleStatChange = (value, label) => {
    setSelectedParkingZone(value);
    parkingTransactionService
      .getAllIncome(value)
      .then((response) => {
        setData(response.data);
        setParkingZoneData(response.data);
        getBookedData(label);
      })
      .catch((error) => {
        console.error('Error fetching data:', error);
      });
  };

  useEffect(() => {
    const filteredData = ParkingZoneData.filter((item) => item.parkingZoneId === selectedParkingZone);
    parkingZoneService
      .getAllParkingZoneByOwnerId(account.UserId)
      .then((response) => {
        setParkingZoneOptions(response.data);
      })
      .catch((error) => {
        console.error('Error fetching parking zones:', error);
      });

    if (dateRange === 'month') {
      const daysInMonth = new Date(new Date().getFullYear(), new Date().getMonth() + 1, 0).getDate();
      const thisMonthData = Array.from({ length: daysInMonth }, (_, i) => ({
        name: `Ngày ${i + 1}`,
        income: filteredData.find((item) => new Date(item.incomeDate).getDate() === i + 1)?.income || 0,
      }));
      setData(thisMonthData);
    } else if (dateRange === 'year') {
      const thisYearData = Array.from({ length: 12 }, (_, i) => ({
        name: `Tháng ${i + 1}`,
        income: filteredData
          .filter((item) => new Date(item.incomeDate).getMonth() === i)
          .reduce((total, item) => total + item.income, 0),
      }));
      setData(thisYearData);
    } else if (dateRange === 'all') {
      const allTimeData = [];
      const groupedData = {};
      filteredData.forEach((item) => {
        const incomeYear = new Date(item.incomeDate).getFullYear();
        if (!groupedData[incomeYear]) {
          groupedData[incomeYear] = 0;
        }
        groupedData[incomeYear] += item.income;
      });

      for (const year in groupedData) {
        allTimeData.push({
          name: year,
          income: groupedData[year],
        });
      }

      setData(allTimeData);
    }

    const totalIncome = data.reduce((total, entry) => total + entry.income, 0);

    const averageMonthlyIncome = data.reduce((total, entry) => total + entry.income, 0) / 12;

    const averageYearlyIncome = data.length > 0 ? totalIncome / data.length : 0;

    setTotalIncome(totalIncome);
    setAverageMonthlyIncome(averageMonthlyIncome);
    setAverageYearlyIncome(averageYearlyIncome);
  }, [dateRange, selectedParkingZone, ParkingZoneData]);

  const getBookedData = async (value) => {
    await parkingZoneOwnerService
      .getBookedOverview({ value })
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
  };

  return (
    <div className={styles.dashboard}>
      <Select className={styles.selectBoxParking} value={selectedParkingZone === '' ? '' : selectedParkingZone} onChange={handleStatChange}>
        <Option value="" disabled={selectedParkingZone !== ''}>
          Chọn bãi đỗ xe
        </Option>
        {ParkingZoneOptions.map((option) => (
          <Option key={option.value} value={option.value}>
            {option.label}
          </Option>
        ))}
      </Select>
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
              <h3>Thống kê thu nhập theo giờ</h3>
              <Line data={chartData} options={options} />
            </div>
          </div>
        )}
      </Fragment>



      <Row style={{ marginTop: '50px' }} gutter={24}>
        <Col span={8}>
          <Card className={styles.cardTitle} title="Tổng Thu Nhập" style={{ backgroundColor: '#e8f0fe' }}>
            <p>{totalIncome} đồng</p>
          </Card>
        </Col>
        <Col span={8}>
          <Card className={styles.cardTitle} title="Trung Bình Hàng Tháng" style={{ backgroundColor: '#f6e4d8' }}>
            <p>{averageMonthlyIncome} đồng</p>
          </Card>
        </Col>
        <Col span={8}>
          <Card className={styles.cardTitle} title="Trung Bình Hàng Năm" style={{ backgroundColor: '#e5f1da' }}>
            <p>{averageYearlyIncome} đồng</p>
          </Card>
        </Col>
      </Row>
      <Card className={styles.CardBelow} title="Thống kê thu nhập">
        <Select className={styles.selectBoxDate} value={dateRange} onChange={handleDateRangeChange}>
          <Option value="month">This Month</Option>
          <Option value="year">This Year</Option>
          <Option value="all">All Time</Option>
        </Select>
        {data.length === 0 ? (
          <Empty description="No data available" />
        ) : (
          <BarChart className={styles.chart} width={1000} height={300} data={data}>
            <XAxis dataKey="name" />
            <YAxis />
            <CartesianGrid strokeDasharray="3 3" />
            <Tooltip />
            <Legend />
            <Bar dataKey="income" fill="#8884d8" />
          </BarChart>
        )}
      </Card>
    </div>
  );
}

export default IncomeDashboard;
