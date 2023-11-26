import { AutoComplete, Button, Col, Pagination, Row, Space } from 'antd';
import { useViewParkingZoneContext } from '@/hooks/useContext/viewParkingZone.context';
import { ReloadOutlined } from '@ant-design/icons';

const ParkingZonesHeader = () => {
  const { viewValues, setViewValues } = useViewParkingZoneContext();

  return (
    <Row gutter={8} style={{ marginBottom: '10px' }}>
      <Col span={8}>
        <Space.Compact style={{ width: '100%' }}>
          <Button icon={<ReloadOutlined />} onClick={() => setViewValues({ ...viewValues, reload: true })} />
          <AutoComplete
            style={{ width: '100%' }}
            onSearch={(value) => setViewValues({ ...viewValues, searchValue: value })}
            value={viewValues.searchValue}
            placeholder="Nhập để tìm kiếm nhà xe"
          />
        </Space.Compact>
      </Col>
      <Col span={8}></Col>

      <Col span={8} style={{ textAlign: 'end' }}>
        <Pagination
          current={viewValues.currentPage}
          total={viewValues.totalItems}
          pageSize={viewValues.pageSize}
          showSizeChanger={true}
          onChange={(page, pageSize) => {
            console.log(page);
            setViewValues({ ...viewValues, currentPage: page, pageSize: pageSize });
          }}
        />
      </Col>
    </Row>
  );
};
export default ParkingZonesHeader;
