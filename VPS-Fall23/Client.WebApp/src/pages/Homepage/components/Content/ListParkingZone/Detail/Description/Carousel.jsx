import { Carousel, Image } from "antd"

const ParkingZoneDescriptionCarousel = ({ parkingZoneImages }) => {
    return (<Image.PreviewGroup>
        <Carousel className="mb-[10px]" autoplay dotPosition="top">
            {parkingZoneImages?.map((img, index) => (
                <Image key={index} src={img} />
            ))}
        </Carousel>
    </Image.PreviewGroup>)
}
export default ParkingZoneDescriptionCarousel