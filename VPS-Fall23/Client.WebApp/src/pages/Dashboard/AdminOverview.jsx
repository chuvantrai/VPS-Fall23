import { Progress, Card, Typography } from 'antd';
import { Fragment } from 'react';

const { Text } = Typography;

function AdminOverview() {
    return (
        <Fragment>
            <Card title="Người dùng" bordered={true} style={{ width: 200 }}>
                {/* <p>Tổng vé xe: {bookedData.doneCheckInOut}</p> */}
                <Text strong>
                    Doanh thu:{' '}
                    {/* {bookedData.monthCash.toLocaleString('vi-VN', {
                        style: 'currency',
                        currency: 'VND',
                    })} */}
                </Text>
            </Card>
            <Card title="Người dùng" bordered={true} style={{ width: 200 }}>
                {/* <p>Tổng vé xe: {bookedData.doneCheckInOut}</p> */}
                <Text strong>
                    Doanh thu:{' '}
                    {/* {bookedData.monthCash.toLocaleString('vi-VN', {
                        style: 'currency',
                        currency: 'VND',
                    })} */}
                </Text>
            </Card>
        </Fragment>
    )
}

export default AdminOverview
