import React, { useState, useEffect } from 'react';
import { Card, Row, Col, Select, DatePicker } from 'antd';
import { BarChart, Bar, XAxis, YAxis, Tooltip, Legend, CartesianGrid } from 'recharts';
import styles from './IncomeDashboard.module.scss';
import { getAccountJwtModel } from '@/helpers';

import useParkingZoneService from '@/services/parkingZoneService';
import useParkingTransactionService from '@/services/parkingTransactionSerivce';


function IncomeDashboard() {

  const parkingZoneService = useParkingZoneService();
  const parkingTransactionService = useParkingTransactionService();
  const { RangePicker } = DatePicker;
  const { Option } = Select;
  const account = getAccountJwtModel();
  const [selectedStat, setSelectedStat] = useState(null);
  const [dateRange, setDateRange] = useState('all');
  const [data, setData] = useState([]);
  const [ParkingZoneOptions, setParkingZoneOptions] = useState([]);
  const [selectedParkingZone, setSelectedParkingZone] = useState('');
  const handleDateRangeChange = (value) => {
    setDateRange(value);
  };

  ParkingZoneData = [];

  const handleStatChange = (value) => {
    setSelectedParkingZone(value);
    parkingTransactionService.getAllIncome(value)
      .then((response) => {
        setData(response.data);
        ParkingZoneData = response.data;
      })
      .catch((error) => {
        console.error('Error fetching data:', error);
      });
  };

  useEffect(() => {

    parkingZoneService.getAllParkingZoneByOwnerId(account.UserId)
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
        income: Math.floor(Math.random() * 1000),
      }));
      setData(thisMonthData);
    } else if (dateRange === 'year') {
      const thisYearData = Array.from({ length: 12 }, (_, i) => ({
        name: `Tháng ${i + 1}`,
        income: Math.floor(Math.random() * 1000),
      }));
      setData(thisYearData);
    } else if (dateRange === 'all') {
      // setData(initialData);
    }
  }, [dateRange], [selectedStat]);
  return (

    <div className={styles.dashboard}>
      <Select
        className={styles.selectBoxParking}
        value={selectedParkingZone}
        onChange={handleStatChange}
      >
        {ParkingZoneOptions.map((option) => (
          <Option key={option.value} value={option.value}>
            {option.label}
          </Option>
        ))}
      </Select>

      <Row gutter={24}>
        <Col span={8}>
          <Card className={styles.cardTitle} title="Tổng Thu Nhập">
            <p>{data.reduce((total, entry) => total + entry.income, 0)} đồng</p>
          </Card>
        </Col>
        <Col span={8}>
          <Card className={styles.cardTitle} title="Trung Bình Hàng Tháng">
            {/* <p>{averageMonthlyData} đồng</p> */}
          </Card>
        </Col>
        <Col span={8}>
          <Card className={styles.cardTitle} title="Trung Bình Hàng Năm">
            {/* <p>{averageMonthlyData} đồng</p> */}
          </Card>
        </Col>
      </Row>
      <Card className={styles.CardBelow} title="Thống kê thu nhập">
        <Select
          className={styles.selectBoxDate}
          value={dateRange}
          onChange={handleDateRangeChange}
        >
          <Option value="month">This Month</Option>
          <Option value="year">This Year</Option>
          <Option value="all">All Time</Option>
        </Select>
        <BarChart className={styles.chart} width={1000} height={300} data={data}>
          <XAxis dataKey="name" />
          <YAxis />
          <CartesianGrid strokeDasharray="3 3" />
          <Tooltip />
          <Legend />
          <Bar dataKey="income" fill="#8884d8" />
        </BarChart>
      </Card>


    </div>
  );
}

export default IncomeDashboard;
