import { Carousel, Image } from "antd"
import classNames from 'classnames/bind';
import styles from '@/pages/Homepage/components/Content/ListParkingZone/Detail/Description/Carousel.module.css';
const cx = classNames.bind(styles);

const ParkingZoneDescriptionCarousel = ({ parkingZoneImages }) => {

    return (<Image.PreviewGroup>
        <Carousel
          className={cx('mb-[10px] Description-Carousel')}
          dotPosition="top">
            {parkingZoneImages?.map((img, index) => (
                <Image className={cx('')} key={index} src={img} />
            ))}
        </Carousel>
    </Image.PreviewGroup>)
}
export default ParkingZoneDescriptionCarousel