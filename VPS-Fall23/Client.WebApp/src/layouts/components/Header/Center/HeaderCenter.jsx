import { Cascader, Col, Form, Input, Row, Select, Space } from 'antd';
import styles from './HeaderCenter.module.scss';
import classNames from 'classnames/bind';


const cx = classNames.bind(styles);
const HeaderCenter = () => {
    return (<div className={cx('wrapper')}>
        <Row gutter={2}>
            <Col md={16} sm={24}>
                <Cascader
                    style={{ width: "100%" }}
                    placeholder="Chọn địa điểm bạn muốn đến"
                    changeOnSelect />
            </Col>
            <Col md={8} sm={24}>
                <Space align='center'>
                    <Input.Search style={{ display: "block" }} placeholder="Hoặc nhập địa chỉ cụ thể" />
                </Space>

            </Col>
        </Row>
    </div>)
}
export default HeaderCenter