import { Fragment, useState, useEffect } from 'react';
import { Card, Row, Col, Select, DatePicker, Empty, Typography } from 'antd';
import { BarChart, Bar, XAxis, YAxis, Tooltip, Legend, CartesianGrid } from 'recharts';
import styles from './IncomeDashboard.module.scss';
import { getAccountJwtModel } from '@/helpers';

import useParkingZoneService from '@/services/parkingZoneService';
import useParkingTransactionService from '@/services/parkingTransactionSerivce';

function IncomeDashboard({ selectedParkingZone, ParkingZoneData }) {


  const parkingZoneService = useParkingZoneService();
  const parkingTransactionService = useParkingTransactionService();
  const { RangePicker } = DatePicker;
  const { Option } = Select;
  const account = getAccountJwtModel();
  const [totalIncome, setTotalIncome] = useState(0);
  const [averageMonthlyIncome, setAverageMonthlyIncome] = useState(0);
  const [averageYearlyIncome, setAverageYearlyIncome] = useState(0);
  const [dateRange, setDateRange] = useState('all');
  const [data, setData] = useState([]);
  // const [ParkingZoneOptions, setParkingZoneOptions] = useState([]);
  // const [ParkingZoneData, setParkingZoneData] = useState([]);

  const handleDateRangeChange = (value) => {
    setDateRange(value);
  };

  // const handleStatChange = (value) => {
  //   setSelectedParkingZone(value);
  //   parkingTransactionService
  //     .getAllIncome(value)
  //     .then((response) => {
  //       setData(response.data);
  //       setParkingZoneData(response.data);
  //     })
  //     .catch((error) => {
  //       console.error('Error fetching data:', error);
  //     });
  // };

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

  return (
    <Fragment>
      {/* <Select className={styles.selectBoxParking} value={selectedParkingZone === '' ? '' : selectedParkingZone} onChange={handleStatChange}>
        <Option value="" disabled={selectedParkingZone !== ''}>
          Tất cả bãi đỗ xe
        </Option>
        {ParkingZoneOptions.map((option) => (
          <Option key={option.value} value={option.value}>
            {option.label}
          </Option>
        ))}
      </Select> */}
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
    </Fragment>
  );
}

export default IncomeDashboard;
