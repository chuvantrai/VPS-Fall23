import { Fragment, useState, useEffect } from 'react';
import { Card, Row, Col, Select, DatePicker, Empty, Typography } from 'antd';
import { BarChart, Bar, XAxis, YAxis, Tooltip, Legend, CartesianGrid } from 'recharts';
import styles from './IncomeDashboard.module.scss';
import { getAccountJwtModel } from '@/helpers';

import useParkingZoneService from '@/services/parkingZoneService';
import useParkingTransactionService from '@/services/parkingTransactionSerivce';

function IncomeDashboard({ selectedParkingZone }) {
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
  const [ParkingZoneData, setParkingZoneData] = useState([]);

  const handleDateRangeChange = (value) => {
    let temporaryData = [];

    if (value === 'month') {
      const daysInMonth = new Date(new Date().getFullYear(), new Date().getMonth() + 1, 0).getDate();

      for (let day = 1; day <= daysInMonth; day++) {
        const filteredDataForDay = ParkingZoneData.filter((item) => new Date(item.incomeDate).getDate() === day);
        console.log(filteredDataForDay);
        const totalIncomeForDay = filteredDataForDay.reduce((total, item) => total + item.income, 0);

        temporaryData.push({
          name: `Ngày ${day}`,
          income: totalIncomeForDay,
        });
      }
    } else if (value === 'year') {
      for (let month = 1; month <= 12; month++) {
        const filteredDataForMonth = ParkingZoneData.filter(
          (item) => new Date(item.incomeDate).getMonth() === month - 1,
        );

        const totalIncomeForMonth = filteredDataForMonth.reduce((total, item) => total + item.income, 0);

        temporaryData.push({
          name: `Tháng ${month}`,
          income: totalIncomeForMonth,
        });
      }
    } else if (value === 'all') {
      const groupedData = {};
      ParkingZoneData.forEach((item) => {
        const incomeYear = new Date(item.incomeDate).getFullYear();
        if (!groupedData[incomeYear]) {
          groupedData[incomeYear] = 0;
        }
        groupedData[incomeYear] += item.income;
      });

      for (const year in groupedData) {
        temporaryData.push({
          name: year,
          income: groupedData[year],
        });
      }
    }
    console.log(temporaryData);
    setData(temporaryData);
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
  const calculateStatistics = (data) => {
    if (!data || data.length === 0) {
      setTotalIncome(0);
      setAverageMonthlyIncome(0);
      setAverageYearlyIncome(0);
      return;
    }

    const totalIncome = data.reduce((total, entry) => total + entry.income, 0);
    const roundedTotalIncome = Math.round(totalIncome / 10000) * 10000;

    const averageMonthlyIncome = totalIncome / 12;
    const roundedAverageMonthlyIncome = Math.round(averageMonthlyIncome / 10000) * 10000;

    const averageYearlyIncome = totalIncome / data.length;
    const roundedAverageYearlyIncome = Math.round(averageYearlyIncome / 10000) * 10000;

    setTotalIncome(roundedTotalIncome);
    setAverageMonthlyIncome(roundedAverageMonthlyIncome);
    setAverageYearlyIncome(roundedAverageYearlyIncome);
  };

  useEffect(() => {
    calculateStatistics();
  }, [dateRange, selectedParkingZone]);

  useEffect(() => {
    parkingTransactionService
      .getAllIncome(selectedParkingZone, account.UserId)
      .then((response) => {
        setData(response.data);
        setParkingZoneData(response.data);
        calculateStatistics(response.data);
        const filteredData = response.data;
        let temporaryData = [];

        if (dateRange === 'month') {
          const daysInMonth = new Date(new Date().getFullYear(), new Date().getMonth() + 1, 0).getDate();

          for (let day = 1; day <= daysInMonth; day++) {
            const filteredDataForDay = filteredData.filter((item) => new Date(item.incomeDate).getDate() === day);
            console.log(filteredDataForDay);
            const totalIncomeForDay = filteredDataForDay.reduce((total, item) => total + item.income, 0);

            temporaryData.push({
              name: `Ngày ${day}`,
              income: totalIncomeForDay,
            });
          }
        } else if (dateRange === 'year') {
          for (let month = 1; month <= 12; month++) {
            const filteredDataForMonth = filteredData.filter(
              (item) => new Date(item.incomeDate).getMonth() === month - 1,
            );

            const totalIncomeForMonth = filteredDataForMonth.reduce((total, item) => total + item.income, 0);

            temporaryData.push({
              name: `Tháng ${month}`,
              income: totalIncomeForMonth,
            });
          }
        } else if (dateRange === 'all') {
          const groupedData = {};
          filteredData.forEach((item) => {
            const incomeYear = new Date(item.incomeDate).getFullYear();
            if (!groupedData[incomeYear]) {
              groupedData[incomeYear] = 0;
            }
            groupedData[incomeYear] += item.income;
          });

          for (const year in groupedData) {
            temporaryData.push({
              name: year,
              income: groupedData[year],
            });
          }
        }
        setData(temporaryData);
      })
      .catch((error) => {
        console.error('Error fetching data:', error);
      });

    const totalIncome = data.reduce((total, entry) => total + entry.income, 0);
    const roundedTotalIncome = Math.round(totalIncome / 10000) * 10000;

    const averageMonthlyIncome = totalIncome / 12;
    const roundedAverageMonthlyIncome = Math.round(averageMonthlyIncome / 10000) * 10000;

    const averageYearlyIncome = totalIncome / data.length;
    const roundedAverageYearlyIncome = Math.round(averageYearlyIncome / 10000) * 10000;

    setTotalIncome(roundedTotalIncome);
    setAverageMonthlyIncome(roundedAverageMonthlyIncome);
    setAverageYearlyIncome(roundedAverageYearlyIncome);
  }, [dateRange, selectedParkingZone]);

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
          <BarChart className={styles.chart} width={1200} height={300} data={data}>
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
