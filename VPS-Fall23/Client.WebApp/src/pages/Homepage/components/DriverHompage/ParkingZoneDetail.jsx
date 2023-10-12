import { Button, Carousel, Image, Modal } from "antd"


const ParkingZoneDetail = ({ parkingZone, isShow, bookingCallback, onCloseCallback, }) => {
    return (<Modal
        open={isShow}
        onCancel={onCloseCallback}
        onOk={bookingCallback}
        okText="Đặt vé"
        okButtonProps={{
            style: {
                backgroundColor: '#1677ff',
            }
        }}
        closable={true}
    >
        <Carousel>
            <Image src="http://210.211.127.113:9003/nghianv-vps-public/parking-zone-images/290e1476-aa4f-4bd6-8a23-e7167e1d0417/3913678a-05c4-48ea-89f1-12771efd96ab/3913678a-05c4-48ea-89f1-12771efd96ab-0..jpg">

            </Image>
        </Carousel>
    </Modal>)
}
export default ParkingZoneDetail