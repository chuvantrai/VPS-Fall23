import { List, Table } from "antd"
import { useCallback, useEffect, useState } from "react"
import ParkingZoneAbsentActions from "./Actions";
import useParkingZoneAbsentServices from "@/services/parkingZoneAbsentServices";
import { useViewParkingZoneContext } from "../../../../../../../../hooks/useContext/viewParkingZone.context";
import dayjs from "dayjs";

const ParkingZoneAbsentList = () => {
    const { detailInfo, setDetailInfo } = useViewParkingZoneContext();
    const parkingZoneAbsentService = useParkingZoneAbsentServices();
    const [absents, setAbsents] = useState([]);

    useEffect(() => {
        getData()
    }, [])
    const getData = useCallback(() => {
        parkingZoneAbsentService
            .getAbsents(detailInfo.parkingZone.id)
            .then(res => setAbsents(res.data))
    }, [])
    const columns = [
        {
            title: 'Ngày đóng cửa',
            dataIndex: 'from',
            key: 'from',
            render: (value) => value ? dayjs(value).format("DD/MM/YYYY") : ''
        },
        {
            title: 'Ngày mở cửa trở lại',
            dataIndex: "to",
            key: 'to',
            render: (value) => value ? dayjs(value).format("DD/MM/YYYY") : ''
        },
        {
            title: "Lí do đóng cửa",
            dataIndex: 'reason',
            key: 'reason'
        },
        {
            title: "Ngày tạo",
            dataIndex: "createdAt",
            key: "createdAt",
            render: (value) => value ? dayjs(value).format("DD/MM/YYYY") : ''
        },
        {
            title: '',
            render: (_, record) => <ParkingZoneAbsentActions record={record} reloadCallback={getData} />
        }
    ]

    return <Table
        dataSource={absents}
        columns={columns}
        tableLayout="fixed"
    />
}
export default ParkingZoneAbsentList