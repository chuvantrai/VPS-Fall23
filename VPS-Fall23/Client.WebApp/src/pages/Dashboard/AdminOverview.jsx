import { Fragment, useState, useEffect } from 'react';
import { Card, Typography } from 'antd';
import {
    Chart, CategoryScale, LinearScale, PointElement, LineElement, Title, Tooltip, Legend,
} from 'chart.js';
import { Line } from 'react-chartjs-2';

const { Text } = Typography;
import useParkingZoneService from '@/services/parkingZoneService';

function AdminOverview() {
    Chart.register(CategoryScale, LinearScale, PointElement, LineElement, Title, Tooltip, Legend);
    const parkingZoneService = useParkingZoneService();
    const [data, setData] = useState();

    const [chartData, setChartData] = useState({
        labels: ['1h', '1day', '1W', '1M', '1Y', 'AllTime'],
        datasets: [
            {
                label: 'Customer',
                data: [0, 0, 0, 0, 0, 0],
                borderColor: 'rgba(75, 192, 192, 1)',
                fill: false,
            },
        ],
    });

    useEffect(() => {
        parkingZoneService
            .getAdminOverview()
            .then((res) => {
                setData(res.data);
                const chart = {
                    datasets: [
                        {
                            label: 'Hoạt động khách hàng',
                            data: [res.data.customerData.oneHours, res.data.customerData.oneDay,
                            res.data.customerData.oneWeek, res.data.customerData.oneMonth, res.data.customerData.oneYear, res.data.customerData.totalCustomer],
                            borderColor: 'rgb(107, 36, 12)',
                            fill: false,
                        },
                        {
                            label: 'Chủ bãi đổ xe',
                            data: [res.data.ownerData.oneHours, res.data.ownerData.oneDay,
                            res.data.ownerData.oneWeek, res.data.ownerData.oneMonth, res.data.ownerData.oneYear, res.data.ownerData.totalOwner],
                            borderColor: 'rgb(38, 80, 115)',
                            fill: false,
                        },
                        {
                            label: 'Bãi đỗ xe',
                            data: [res.data.parkingZoneData.oneHours, res.data.parkingZoneData.oneDay,
                            res.data.parkingZoneData.oneWeek, res.data.parkingZoneData.oneMonth, res.data.parkingZoneData.oneYear, res.data.parkingZoneData.totalParkingZone],
                            borderColor: 'rgb(45, 149, 150)',
                            fill: false,
                        },
                    ],
                };
                setChartData(chart);
            })
            .catch((error) => {
                console.error('Error fetching parking zones:', error);
            });
    }, []);

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
                            return `${label}: ${context.parsed.y.toLocaleString('vi-VN')}`;
                        }
                        return null;
                    },
                },
            },

        }
    };

    return (
        <Fragment>
            {data !== null && data !== undefined && (
                <div className='block'>
                    <div className='flex' style={{ width: "800px" }}>
                        <Card className='mr-5' title="Người dùng" bordered={true} style={{ width: 300 }}>
                            <div>
                                <Text strong>
                                    Tổng: {data.customerTotal}
                                </Text>
                            </div>
                            <div>
                                <Text>
                                    Hoạt động trong tháng này: {data.activeCustomer}
                                </Text>
                            </div>

                            <Text>
                                Người dùng mới trong tháng: {data.newCustomer}
                            </Text>
                        </Card>
                        <Card title="Bãi đỗ xe" bordered={true} style={{ width: 300 }}>
                            <div>
                                <Text strong>
                                    Số lượng chủ sở hữu: {data.ownerData.totalOwner}
                                </Text>
                            </div>
                            <div>
                                <Text strong>
                                    Số lượng bãi đỗ xe: {data.parkingZoneData.totalParkingZone}
                                </Text>
                            </div>
                            <Text strong>
                                ~~
                            </Text>
                        </Card>
                    </div>

                    <div className="mt-6" style={{ height: "500px" }}>
                        <Line data={chartData} options={options} />
                    </div>
                </div>
            )}
        </Fragment>
    )
}

export default AdminOverview
