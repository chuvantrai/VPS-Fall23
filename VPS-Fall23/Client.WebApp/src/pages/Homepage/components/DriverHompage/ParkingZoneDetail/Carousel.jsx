import { Carousel, Image } from "antd";


const ParkingZoneDetailCarousel = ({ imageLinks }) => {
    return (<Image.PreviewGroup>
        <Carousel adaptiveHeight={true} autoplay dotPosition='top'>
            {imageLinks.map((val, index) => {
                return <Image width={'100%'} style={{ objectFit: "cover" }} key={index} src={val}></Image>;
            })}
        </Carousel>
    </Image.PreviewGroup>)
}
export default ParkingZoneDetailCarousel