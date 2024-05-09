import html2canvas from "html2canvas";
import {getDateFormatForFile} from "./date-util";
import {jsPDF} from "jspdf";
import {messageService} from "../message-service";

/**
 * Export PDF
 * @param domTarget dom Identity
 * @param title Title of PDF File
 */
export const exportPdfFile = async (domTarget: string, title: string) => {
  try {

    // Get Native Html elements
    const element = document.getElementById(domTarget) as HTMLElement;

    // Transform to canvas
    const canvas = await html2canvas(element, {
      onclone: function (clonedDoc) {
        clonedDoc.getElementById('capture-area').style.padding = '20px';
      }
    });

    // Get width and Height from canvas
    const width = canvas.width;
    const height = canvas.height;

    // To Image from stream
    const image = canvas.toDataURL('image/png').replace('image/png', 'image/octet-stream');
    console.log(image)

    // Generate PDF
    const pdf = new jsPDF({
      orientation: 'p',
      unit: 'px',
      format: [width, height]
    });

    pdf.addImage(image, 'PNG', 0, 0, width, height);
    pdf.save(`${getDateFormatForFile()}_${title}.pdf`);
  }catch (e) {
    messageService.showError("PDF 를 출력하는 도중 에러가 발생했습니다\n지속적으로 발생하면 새로고침 해주세요.")
    console.error(e);
  }
}
