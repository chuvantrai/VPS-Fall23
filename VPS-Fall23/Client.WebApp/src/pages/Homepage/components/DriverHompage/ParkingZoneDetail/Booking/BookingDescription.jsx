import { Descriptions, Statistic } from "antd"
import dayjs from "dayjs"


const BookingDescription = ({ pricePerHour, ioTime, discount = 0 }) => {
    const hours = Math.abs(dayjs(ioTime[0]).diff(ioTime[1], 'hours'))
    const total = pricePerHour * hours;
    const items = [
        {
            key: 1,
            label: 'Đơn giá',
            children: <Statistic valueStyle={{ fontSize: '1.1em' }} value={pricePerHour} suffix='VNĐ'></Statistic>,
        },
        {
            key: 2,
            label: 'Số giờ gửi',
            children: <Statistic valueStyle={{ fontSize: '1.1em' }} value={hours} suffix='h'></Statistic>,
        },
        {
            key: 3,
            label: 'Tổng',
            children: <Statistic valueStyle={{ fontSize: '1.1em' }} value={total} suffix='VNĐ' />,
        },
        {
            key: 4,
            label: 'Mã giảm giá',
            children: <Statistic valueStyle={{ fontSize: '1.1em' }} value={discount} prefix='-' suffix='%'></Statistic>,
        },
        {
            key: 5,
            label: 'Tổng tiền',
            children: <Statistic valueStyle={{ fontSize: '1.1em' }} value={total - total * discount / 100} suffix='VNĐ'></Statistic>,
        },
    ]

    return (ioTime && ioTime[0] && ioTime[1] && <Descriptions bordered
        items={items}
        size='small'
        column={1}
        title='Tổng cộng'
    >

    </Descriptions>)
}
export default BookingDescription