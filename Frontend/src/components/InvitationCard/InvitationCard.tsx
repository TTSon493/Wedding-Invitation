import { forwardRef } from "react";

interface InvitationData {
  message: string;
  bride: string;
  groom: string;
  date: string;
  time: string;
  venue: string;
  location: string;
  additionalInfo?: string;
}

interface Design {
  font: string;
  fontSize: number;
  textColor: string;
  backgroundColor: string;
  backgroundImage: string;
  textAlign: "left" | "center" | "right";
}

interface InvitationCardProps {
  invitationData: InvitationData;
  design: Design;
  selectedSticker: number | null;
  className?: string;
}

const InvitationCard = forwardRef<HTMLDivElement, InvitationCardProps>(
  ({ invitationData, design, selectedSticker, className }, ref) => {
    return (
      <div
        ref={ref}
        className={className}
        style={{
          textAlign: design.textAlign,
          backgroundColor: design.backgroundColor,
          backgroundImage: `url(${design.backgroundImage})`,
          backgroundSize: "cover",
          backgroundPosition: "center",
          padding: "2rem",
          borderRadius: "8px",
          boxShadow: "0 4px 8px rgba(0, 0, 0, 0.1)",
        }}>
        <h2
          style={{
            fontFamily: design.font,
            fontSize: design.fontSize,
            color: design.textColor,
          }}>
          {invitationData.message}
        </h2>
        <h3
          style={{
            fontFamily: design.font,
            fontSize: design.fontSize,
            color: design.textColor,
          }}>
          {invitationData.bride} & {invitationData.groom}
        </h3>
        <p
          style={{
            fontFamily: design.font,
            fontSize: design.fontSize,
            color: design.textColor,
          }}>
          {invitationData.date}
        </p>
        <p
          style={{
            fontFamily: design.font,
            fontSize: design.fontSize,
            color: design.textColor,
          }}>
          {invitationData.time}
        </p>
        <p
          style={{
            fontFamily: design.font,
            fontSize: design.fontSize,
            color: design.textColor,
          }}>
          {invitationData.venue}, {invitationData.location}
        </p>
        <p
          style={{
            fontFamily: design.font,
            fontSize: design.fontSize,
            color: design.textColor,
          }}>
          {invitationData.additionalInfo}
        </p>
        {selectedSticker && (
          <img
            src={`/images/sticker${selectedSticker}.png`}
            alt={`Sticker ${selectedSticker}`}
            className='mt-4'
            style={{ width: "150px", height: "150px" }}
          />
        )}
      </div>
    );
  }
);

InvitationCard.displayName = "InvitationCard";

export default InvitationCard;
