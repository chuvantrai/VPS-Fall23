import { Modal } from "antd"


const ParkingZoneDetailModal = ({ parkingZone, modalProps, modalContent }) => {

    return (
        <Modal
            {...modalProps}
            centered
            width={'40vw'}
        // footer={() => (
        //     <Space>
        //         {account.RoleId === '2' && (
        //             <Switch
        //                 checkedChildren="Full"
        //                 unCheckedChildren="Available"
        //                 onChange={handleChangeSwitch}
        //                 checked={checkedState}
        //             />
        //         )}
        //         <Button className="bg-[#1677ff] text-white" onClick={handleViewOk}>
        //             OK
        //         </Button>
        //     </Space>
        // )}
        >
            {modalContent}
            {/* <Image.PreviewGroup>
                <Carousel className="mb-[10px]" autoplay dotPosition="top">
                    {parkingZoneDetail.parkingZoneImages?.map((img, index) => (
                        <Image key={index} src={img} />
                    ))}
                </Carousel>
            </Image.PreviewGroup>
            <Descriptions bordered items={descItems} column={{ xs: 1, sm: 1, md: 1, lg: 1, xl: 2, xxl: 2 }} /> */}
        </Modal>
    )
}
export default ParkingZoneDetailModal